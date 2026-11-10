using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace KlamWA.Servicios.Rest;

public static class HttpUtils
{
    public static async Task<TRespuesta?> DoGet<TRespuesta>(HttpClient client, string ruta)
    {
        using var response = await client.GetAsync(ruta);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task<TRespuesta?> DoPost<TSolicitud, TRespuesta>(
        HttpClient client, string ruta, TSolicitud datos)
    {
        using var response = await client.PostAsJsonAsync(ruta, datos);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return default;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task<TRespuesta?> DoGetAutenticado<TRespuesta>(
        HttpClient client, string ruta, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, ruta);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request);
        await VerificarRespuestaAsync(response);
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task DoPostAutenticado<TSolicitud>(
        HttpClient client, string ruta, string token, TSolicitud datos)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, ruta)
        {
            Content = JsonContent.Create(datos)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request);
        await VerificarRespuestaAsync(response);
    }

    public static async Task<TRespuesta?> DoPostAutenticado<TSolicitud, TRespuesta>(
        HttpClient client, string ruta, string token, TSolicitud datos)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, ruta)
        {
            Content = JsonContent.Create(datos)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        using var response = await client.SendAsync(request);
        await VerificarRespuestaAsync(response);
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task VerificarRespuestaAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        string? mensaje = null;
        try
        {
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (json.RootElement.TryGetProperty("error", out var error))
                mensaje = error.GetString();
        }
        catch (JsonException)
        {
            // Los errores sin cuerpo JSON conservan el código HTTP.
        }
        throw new HttpRequestException(
            string.IsNullOrWhiteSpace(mensaje)
                ? $"El servidor respondió {(int)response.StatusCode} ({response.ReasonPhrase})."
                : mensaje,
            null, response.StatusCode);
    }
}
