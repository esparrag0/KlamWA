using System.Net.Http.Json;

namespace KlamWA.Servicios.Rest;

// Llamadas al backend como en el proyecto del profesor: HttpClient estático y URL completa.
// Cualquier error (respuesta 4xx/5xx o servidor caído) llega a la pantalla como ServicioException,
// con el mensaje {"error": "..."} de la BL, para que ninguna página se caiga.
public static class HttpUtils
{
    private static HttpClient http = new();

    public static async Task<TResponse> DoGet<TResponse>(string url)
    {
        var response = await Enviar(() => http.GetAsync(url));
        await VerificarRespuesta(response);
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    // Descarga de archivos (documentos): contenido y tipo de contenido
    public static async Task<(byte[] Contenido, string TipoContenido)> DoGetArchivo(string url)
    {
        var response = await Enviar(() => http.GetAsync(url));
        await VerificarRespuesta(response);
        string tipo = response.Content.Headers.ContentType?.MediaType ?? "application/octet-stream";
        return (await response.Content.ReadAsByteArrayAsync(), tipo);
    }

    public static async Task<TResponse>
        DoPost<TRequest, TResponse>(string url, TRequest payload)
    {
        var response = await Enviar(() => http.PostAsJsonAsync(url, payload));
        await VerificarRespuesta(response);
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    public static async Task DoPost<TRequest>(string url, TRequest payload)
    {
        var response = await Enviar(() => http.PostAsJsonAsync(url, payload));
        await VerificarRespuesta(response);
    }

    // Acciones sin datos (aprobar, validar, marcar leída): se envía {} para que el backend reciba JSON
    public static async Task DoPost(string url)
    {
        var response = await Enviar(() => http.PostAsJsonAsync(url, new { }));
        await VerificarRespuesta(response);
    }

    public static async Task<TResponse> DoPostMultipart<TResponse>(string url, MultipartFormDataContent contenido)
    {
        var response = await Enviar(() => http.PostAsync(url, contenido));
        await VerificarRespuesta(response);
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    public static async Task<TResponse>
        DoPut<TRequest, TResponse>(string url, TRequest payload)
    {
        var response = await Enviar(() => http.PutAsJsonAsync(url, payload));
        await VerificarRespuesta(response);
        return (await response.Content.ReadFromJsonAsync<TResponse>())!;
    }

    public static async Task DoPut<TRequest>(string url, TRequest payload)
    {
        var response = await Enviar(() => http.PutAsJsonAsync(url, payload));
        await VerificarRespuesta(response);
    }

    public static async Task DoDelete(string url)
    {
        var response = await Enviar(() => http.DeleteAsync(url));
        await VerificarRespuesta(response);
    }

    // Backend apagado o sin respuesta: se avisa en la pantalla en vez de tumbar el circuito
    private static async Task<HttpResponseMessage> Enviar(Func<Task<HttpResponseMessage>> peticion)
    {
        try
        {
            return await peticion();
        }
        catch (HttpRequestException ex)
        {
            throw new ServicioException("No se pudo conectar con el servidor. Verifique que el backend esté desplegado.", null, ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new ServicioException("El servidor no respondió a tiempo.", null, ex);
        }
    }

    // En vez de solo EnsureSuccessStatusCode: el error lleva el mensaje {"error": "..."} de la BL
    // y el código HTTP (EsNoEncontrado para el 404)
    private static async Task VerificarRespuesta(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
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
            throw new ServicioException(error["error"], (int)response.StatusCode);
        }
        throw new ServicioException($"Error del servicio ({(int)response.StatusCode})", (int)response.StatusCode);
    }
}
