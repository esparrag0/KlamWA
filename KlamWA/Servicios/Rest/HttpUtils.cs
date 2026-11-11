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

    // En vez de solo EnsureSuccessStatusCode: el 404 se lanza igual que el profesor (HttpRequestException)
    // y los demás errores llevan el mensaje {"error": "..."} de la BL, para mostrarlo en la pantalla
    private static async Task VerificarRespuesta(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            response.EnsureSuccessStatusCode();
        }
        Dictionary<string, string>? error = null;
        try
        {
            error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        }
        catch (Exception)
        {
            // el cuerpo no era JSON
        }
        if (error != null && error.ContainsKey("error"))
        {
            throw new ServicioException(error["error"]);
        }
        throw new ServicioException($"Error del servicio ({(int)response.StatusCode})");
    }

    // ---- Login y servicios con sesión (HttpClient inyectado y token) ----

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
