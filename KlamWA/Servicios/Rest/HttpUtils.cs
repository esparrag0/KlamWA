using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace KlamWA.Servicios.Rest;

public static class HttpUtils
{
    // ---- Rol 5: llamadas como en el proyecto del profesor (URL completa, HttpClient interno) ----

    private static HttpClient http = new();

    public static async Task<TResponse> DoGet<TResponse>(string url)
    {
        var response = await http.GetAsync(url);
        await VerificarRespuesta(response);
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    public static async Task<TResponse>
        DoPost<TRequest, TResponse>(string url, TRequest payload)
    {
        var response = await http.PostAsJsonAsync(url, payload);
        await VerificarRespuesta(response);
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    public static async Task<TResponse>
        DoPut<TRequest, TResponse>(string url, TRequest payload)
    {
        var response = await http.PutAsJsonAsync(url, payload);
        await VerificarRespuesta(response);
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    public static async Task DoPut<TRequest>(string url, TRequest payload)
    {
        var response = await http.PutAsJsonAsync(url, payload);
        await VerificarRespuesta(response);
    }

    public static async Task DoDelete(string url)
    {
        var response = await http.DeleteAsync(url);
        await VerificarRespuesta(response);
    }

    // El 404 sigue saliendo como HttpRequestException con su código, porque varias
    // pantallas ramifican por ex.StatusCode; lo único que cambia es que el texto ya
    // está en lenguaje de la plataforma en vez de "Response status code does not
    // indicate success". Los demás errores llevan el mensaje {"error": "..."} de la BL.
    private static async Task VerificarRespuesta(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }
        var codigo = (int)response.StatusCode;

        Dictionary<string, string>? error = null;
        try
        {
            error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        }
        catch (Exception)
        {
            // el cuerpo no era JSON
        }
        var delServidor = error != null && error.TryGetValue("error", out var e) ? e : null;
        var mensaje = MensajeDeError.Humanizar(delServidor, codigo);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            throw new HttpRequestException(mensaje, null, HttpStatusCode.NotFound);
        }
        throw new ServicioException(mensaje, codigo);
    }

    // ---- Login y servicios con sesión (HttpClient inyectado y token) ----

    public static async Task<TRespuesta?> DoGet<TRespuesta>(HttpClient client, string ruta)
    {
        using var response = await client.GetAsync(ruta);
        // VerificarRespuestaAsync en lugar de EnsureSuccessStatusCode: lanza la misma
        // HttpRequestException con el mismo código, pero con un mensaje legible.
        await VerificarRespuestaAsync(response);
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task<TRespuesta?> DoPost<TSolicitud, TRespuesta>(
        HttpClient client, string ruta, TSolicitud datos)
    {
        using var response = await client.PostAsJsonAsync(ruta, datos);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return default;

        await VerificarRespuestaAsync(response);
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
            MensajeDeError.Humanizar(mensaje, (int)response.StatusCode),
            null, response.StatusCode);
    }
}
