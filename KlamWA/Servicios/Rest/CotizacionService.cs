using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Globalization;
using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;

namespace KlamWA.Servicios.Rest;

public sealed class CotizacionService(HttpClient client, AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : ICotizacionService
{
    private Task<string> TokenAsync() => tokens.ObtenerAsync(autenticacion);

    public async Task<List<CotizacionDTO>> ListarAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<List<CotizacionDTO>>(client,
            $"cotizaciones/solicitud/{idSolicitud}", await TokenAsync()) ?? [];

    public async Task<CotizacionDTO?> VigenteAsync(int idSolicitud)
    {
        try { return await HttpUtils.DoGetAutenticado<CotizacionDTO>(client,
            $"cotizaciones/solicitud/{idSolicitud}/vigente", await TokenAsync()); }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound) { return null; }
    }

    public async Task<CotizacionCreadaDTO> AdjuntarAsync(int idSolicitud, decimal precioPactado, IBrowserFile archivo)
    {
        var error = ArchivoDocumentoSeleccion.Validar(archivo);
        if (error is not null) throw new InvalidDataException(error);
        using var contenido = new MultipartFormDataContent();
        using var archivoContenido = new StreamContent(archivo.OpenReadStream(ArchivoDocumentoSeleccion.MaximoBytes));
        if (MediaTypeHeaderValue.TryParse(archivo.ContentType, out var tipo)) archivoContenido.Headers.ContentType = tipo;
        contenido.Add(archivoContenido, "archivo", archivo.Name);
        contenido.Add(new StringContent(precioPactado.ToString(CultureInfo.InvariantCulture)), "precioPactado");
        using var peticion = new HttpRequestMessage(HttpMethod.Post, $"solicitudes/{idSolicitud}/cotizaciones")
            { Content = contenido };
        peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await TokenAsync());
        using var respuesta = await client.SendAsync(peticion);
        await HttpUtils.VerificarRespuestaAsync(respuesta);
        return await respuesta.Content.ReadFromJsonAsync<CotizacionCreadaDTO>()
            ?? throw new HttpRequestException("La respuesta de la cotización está vacía.");
    }

    public async Task<ArchivoDocumentoDTO> DescargarAsync(int idCotizacion)
    {
        using var peticion = new HttpRequestMessage(HttpMethod.Get, $"cotizaciones/{idCotizacion}/archivo");
        peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await TokenAsync());
        using var respuesta = await client.SendAsync(peticion);
        await HttpUtils.VerificarRespuestaAsync(respuesta);
        var tipo = respuesta.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
        var extension = tipo switch { "application/pdf" => ".pdf", "image/png" => ".png", "image/jpeg" => ".jpg", _ => "" };
        return new ArchivoDocumentoDTO { Contenido = await respuesta.Content.ReadAsByteArrayAsync(),
            TipoContenido = tipo, Nombre = $"cotizacion-{idCotizacion}{extension}" };
    }

    public async Task DecidirAsync(int idCotizacion, bool aceptar, string? motivo)
    {
        if (aceptar)
            await HttpUtils.DoPostAutenticado(client, $"cotizaciones/{idCotizacion}/aceptacion",
                await TokenAsync(), new { });
        else
            await HttpUtils.DoPostAutenticado(client, $"cotizaciones/{idCotizacion}/rechazo",
                await TokenAsync(), new { motivo });
    }

    public async Task<ElegibilidadCirugiaDTO> ElegibilidadAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<ElegibilidadCirugiaDTO>(client,
            $"solicitudes/{idSolicitud}/elegibilidad-cirugia", await TokenAsync())
        ?? throw new HttpRequestException("La respuesta de elegibilidad está vacía.");
}