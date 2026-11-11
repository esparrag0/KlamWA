using KlamWA.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class DocumentoIngresoService(
    HttpClient client,
    AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : IDocumentoIngresoService
{
    public async Task<List<DocumentoIngresoDTO>> ListarPorSolicitudAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<List<DocumentoIngresoDTO>>(
            client, $"documentos/solicitud/{idSolicitud}", await tokens.ObtenerAsync(autenticacion)) ?? [];

    public async Task<List<DocumentoIngresoDTO>> ListarPropiosAsync(int? idSolicitud = null) =>
        await HttpUtils.DoGetAutenticado<List<DocumentoIngresoDTO>>(
            client, idSolicitud is > 0 ? $"documentos/solicitud/{idSolicitud}/propios" : "documentos/propios",
            await tokens.ObtenerAsync(autenticacion)) ?? [];

    public async Task<RequisitosDocumentosDTO> RequisitosAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<RequisitosDocumentosDTO>(
            client, $"documentos/solicitud/{idSolicitud}/requisitos", await tokens.ObtenerAsync(autenticacion))
        ?? new RequisitosDocumentosDTO { IdSolicitud = idSolicitud };

    public Task RegistrarArchivoAsync(int idSolicitud, string tipoDocumento, IBrowserFile archivo) =>
        EnviarArchivoAsync($"documentos/solicitud/{idSolicitud}/archivo", archivo, tipoDocumento);

    public Task ResubirArchivoAsync(int idDocumento, IBrowserFile archivo) =>
        EnviarArchivoAsync($"documentos/{idDocumento}/resubida", archivo);

    public async Task<ArchivoDocumentoDTO> DescargarAsync(int idDocumento)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"documentos/{idDocumento}/archivo");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer", await tokens.ObtenerAsync(autenticacion));
        using var response = await client.SendAsync(request);
        await HttpUtils.VerificarRespuestaAsync(response);
        var tipo = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
        var extension = tipo switch
        {
            "application/pdf" => ".pdf",
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            _ => ""
        };
        return new ArchivoDocumentoDTO
        {
            Contenido = await response.Content.ReadAsByteArrayAsync(),
            TipoContenido = tipo,
            Nombre = $"documento-{idDocumento}{extension}"
        };
    }

    private async Task EnviarArchivoAsync(string ruta, IBrowserFile archivo, string? tipoDocumento = null)
    {
        const long maxBytes = 5L * 1024 * 1024;
        if (archivo.Size == 0 || archivo.Size > maxBytes)
            throw new InvalidDataException("El archivo debe tener contenido y no superar 5 MB.");
        using var formulario = new MultipartFormDataContent();
        using var contenido = new StreamContent(archivo.OpenReadStream(maxBytes));
        if (!string.IsNullOrWhiteSpace(archivo.ContentType))
            contenido.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);
        formulario.Add(contenido, "archivo", archivo.Name);
        if (tipoDocumento is not null)
            formulario.Add(new StringContent(tipoDocumento), "tipoDocumento");
        using var request = new HttpRequestMessage(HttpMethod.Post, ruta) { Content = formulario };
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer", await tokens.ObtenerAsync(autenticacion));
        using var response = await client.SendAsync(request);
        await HttpUtils.VerificarRespuestaAsync(response);
    }

    public async Task<List<HistorialDocumentoDTO>> ConsultarHistorialAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<List<HistorialDocumentoDTO>>(
            client, $"documentos/solicitud/{idSolicitud}/historial", await tokens.ObtenerAsync(autenticacion)) ?? [];

    public async Task<VerificacionDocumentosDTO> VerificarDocumentosAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<VerificacionDocumentosDTO>(
            client, $"documentos/solicitud/{idSolicitud}/verificacion", await tokens.ObtenerAsync(autenticacion))
            ?? throw new HttpRequestException("La respuesta de verificación está vacía.");

    public async Task ValidarAsync(int idDocumento) =>
        await HttpUtils.DoPostAutenticado(client, $"documentos/{idDocumento}/validacion",
            await tokens.ObtenerAsync(autenticacion), new { });

    public async Task RechazarAsync(int idDocumento, string motivo, bool habilitarResubida) =>
        await HttpUtils.DoPostAutenticado(client, $"documentos/{idDocumento}/rechazo",
            await tokens.ObtenerAsync(autenticacion),
            new { Motivo = motivo, HabilitarResubida = habilitarResubida });
}
