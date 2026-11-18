using KlamWA.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace KlamWA.Servicios.Rest;

public sealed class DocumentoIngresoService(
    HttpClient client,
    AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens,
    ILogger<DocumentoIngresoService> logger) : IDocumentoIngresoService
{
    public async Task<List<DocumentoIngresoDTO>> ListarPorSolicitudAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<List<DocumentoIngresoDTO>>(
            client, $"documentos/solicitud/{idSolicitud}", await tokens.ObtenerAsync(autenticacion)) ?? [];

    public async Task<List<DocumentoIngresoDTO>> ListarPorCirugiaAsync(int idCirugia) =>
        await HttpUtils.DoGetAutenticado<List<DocumentoIngresoDTO>>(
            client, $"documentos/cirugia/{idCirugia}", await tokens.ObtenerAsync(autenticacion)) ?? [];

    public Task AdjuntarOrdenCompraAsync(int idCirugia, IBrowserFile archivo) =>
        EnviarArchivoAsync($"documentos/cirugia/{idCirugia}/orden-compra", archivo);

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
        var error = ArchivoDocumentoSeleccion.Validar(archivo);
        if (error is not null) throw new InvalidDataException(error);
        try
        {
            var token = await tokens.ObtenerAsync(autenticacion);
            using var formulario = new MultipartFormDataContent();
            using var contenido = new StreamContent(archivo.OpenReadStream(ArchivoDocumentoSeleccion.MaximoBytes));
            if (MediaTypeHeaderValue.TryParse(archivo.ContentType, out var tipoContenido))
                contenido.Headers.ContentType = tipoContenido;
            formulario.Add(contenido, "archivo", archivo.Name);
            if (tipoDocumento is not null)
                formulario.Add(new StringContent(tipoDocumento), "tipoDocumento");
            using var request = new HttpRequestMessage(HttpMethod.Post, ruta) { Content = formulario };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            using var response = await client.SendAsync(request);
            await HttpUtils.VerificarRespuestaAsync(response);
        }
        catch (Exception ex) when (ex is IOException or JSException or InvalidOperationException
                                   or TimeoutException or OperationCanceledException)
        {
            logger.LogWarning(ex, "No se pudo leer o enviar el archivo documental seleccionado");
            throw new IOException(ex is OperationCanceledException or TimeoutException
                ? "La carga tardó demasiado. Comprueba si se registró antes de reintentar."
                : "No se pudo leer el archivo seleccionado. Selecciónalo de nuevo e intenta otra vez.", ex);
        }
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
