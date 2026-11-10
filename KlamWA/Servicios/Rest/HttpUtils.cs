using System.Net;
using System.Net.Http.Json;

namespace KlamWA.Servicios.Rest;

// Llamadas REST como en el proyecto del profesor (DoGet, DoPost, DoPut, DoDelete)
public static class HttpUtils
{
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

    public static async Task DoPost<TRequest>(string url, TRequest payload)
    {
        var response = await http.PostAsJsonAsync(url, payload);
        await VerificarRespuesta(response);
    }

    public static async Task DoPost(string url)
    {
        var response = await http.PostAsync(url, null);
        await VerificarRespuesta(response);
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
}
