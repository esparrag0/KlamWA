using KlamWA.DTOs;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;

namespace KlamWA.Servicios.Rest;

// RF07: documentos de ingreso. Quien presenta o revisa (idUsuario, rol, responsable) sale de la sesión.
public class DocumentoIngresoService : IDocumentoIngresoService
{
    private readonly ISesionService _sesion;
    private string BaseUrl { get; set; }

    public DocumentoIngresoService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/documentos";
    }

    public async Task<List<DocumentoIngresoDTO>> ListarPorSolicitudAsync(int idSolicitud) =>
        await HttpUtils.DoGet<List<DocumentoIngresoDTO>>($"{BaseUrl}/solicitud/{idSolicitud}") ?? [];

    public async Task<List<DocumentoIngresoDTO>> ListarPropiosAsync(int? idSolicitud = null)
    {
        string url = idSolicitud is > 0 ? $"{BaseUrl}/solicitud/{idSolicitud}/propios" : $"{BaseUrl}/propios";
        return await HttpUtils.DoGet<List<DocumentoIngresoDTO>>(await ConUsuario(url, false)) ?? [];
    }

    public async Task RegistrarArchivoAsync(int idSolicitud, string tipoDocumento, IBrowserFile archivo) =>
        await EnviarArchivoAsync(await ConUsuario($"{BaseUrl}/solicitud/{idSolicitud}/archivo", true),
            archivo, tipoDocumento);

    public async Task ResubirArchivoAsync(int idDocumento, IBrowserFile archivo) =>
        await EnviarArchivoAsync(await ConUsuario($"{BaseUrl}/{idDocumento}/resubida", true), archivo);

    public async Task<ArchivoDocumentoDTO> DescargarAsync(int idDocumento)
    {
        var (contenido, tipo) = await HttpUtils.DoGetArchivo(await ConUsuario($"{BaseUrl}/{idDocumento}/archivo", false));
        var extension = tipo switch
        {
            "application/pdf" => ".pdf",
            "image/png" => ".png",
            "image/jpeg" => ".jpg",
            _ => ""
        };
        return new ArchivoDocumentoDTO
        {
            Contenido = contenido,
            TipoContenido = tipo,
            Nombre = $"documento-{idDocumento}{extension}"
        };
    }

    private static async Task EnviarArchivoAsync(string url, IBrowserFile archivo, string? tipoDocumento = null)
    {
        const long maxBytes = 5L * 1024 * 1024;
        if (archivo.Size == 0 || archivo.Size > maxBytes)
            throw new ServicioException("El archivo debe tener contenido y no superar 5 MB.", 400);
        using var formulario = new MultipartFormDataContent();
        using var contenido = new StreamContent(archivo.OpenReadStream(maxBytes));
        if (!string.IsNullOrWhiteSpace(archivo.ContentType))
            contenido.Headers.ContentType = new MediaTypeHeaderValue(archivo.ContentType);
        formulario.Add(contenido, "archivo", archivo.Name);
        if (tipoDocumento is not null)
            formulario.Add(new StringContent(tipoDocumento), "tipoDocumento");
        await HttpUtils.DoPostMultipart<System.Text.Json.JsonElement>(url, formulario);
    }

    public async Task<List<HistorialDocumentoDTO>> ConsultarHistorialAsync(int idSolicitud) =>
        await HttpUtils.DoGet<List<HistorialDocumentoDTO>>($"{BaseUrl}/solicitud/{idSolicitud}/historial") ?? [];

    public async Task<VerificacionDocumentosDTO> VerificarDocumentosAsync(int idSolicitud) =>
        await HttpUtils.DoGet<VerificacionDocumentosDTO>($"{BaseUrl}/solicitud/{idSolicitud}/verificacion");

    public async Task ValidarAsync(int idDocumento) =>
        await HttpUtils.DoPost(await ConResponsable($"{BaseUrl}/{idDocumento}/validacion"));

    public async Task RechazarAsync(int idDocumento, string motivo, bool habilitarResubida) =>
        await HttpUtils.DoPost(await ConResponsable($"{BaseUrl}/{idDocumento}/rechazo"),
            new { Motivo = motivo, HabilitarResubida = habilitarResubida });

    // idUsuario y rol de la sesión (y el responsable para la bitácora, si es una escritura)
    private async Task<string> ConUsuario(string url, bool conResponsable)
    {
        var parametros = new Dictionary<string, string?>
        {
            ["idUsuario"] = (await _sesion.ObtenerIdUsuarioAsync()).ToString(),
            ["rol"] = await _sesion.ObtenerRolAsync()
        };
        if (conResponsable)
        {
            parametros["responsable"] = await _sesion.ObtenerUserNameAsync();
        }
        return QueryHelpers.AddQueryString(url, parametros);
    }

    private async Task<string> ConResponsable(string url) =>
        QueryHelpers.AddQueryString(url, "responsable", await _sesion.ObtenerUserNameAsync());
}
