using System.Net;
using System.Text.Json;

namespace KlamWA.Servicios.Rest;


public static class HttpUtils
{
    private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(30) };

    public static async Task<TResponse> DoGet<TResponse>(string url)
    {
        using var respuesta = await Enviar(() => Http.GetAsync(url));
        return await Leer<TResponse>(respuesta);
    }

    public static async Task<TResponse> DoPost<TRequest, TResponse>(string url, TRequest payload)
    {
        using var respuesta = await Enviar(() => Http.PostAsJsonAsync(url, payload));
        return await Leer<TResponse>(respuesta);
    }

    public static async Task DoPost<TRequest>(string url, TRequest payload)
    {
        using var respuesta = await Enviar(() => Http.PostAsJsonAsync(url, payload));
    }

    /// POST sin cuerpo (acciones como marcar leído o registrar un retorno).
    public static async Task DoPost(string url)
    {
        using var respuesta = await Enviar(() => Http.PostAsync(url, null));
    }

    public static async Task<TResponse> DoPut<TRequest, TResponse>(string url, TRequest payload)
    {
        using var respuesta = await Enviar(() => Http.PutAsJsonAsync(url, payload));
        return await Leer<TResponse>(respuesta);
    }

    public static async Task DoPut<TRequest>(string url, TRequest payload)
    {
        using var respuesta = await Enviar(() => Http.PutAsJsonAsync(url, payload));
    }

    public static async Task DoDelete(string url)
    {
        using var respuesta = await Enviar(() => Http.DeleteAsync(url));
    }

    // ------------------------------------------------------------------

    private static async Task<HttpResponseMessage> Enviar(Func<Task<HttpResponseMessage>> llamada)
    {
        HttpResponseMessage respuesta;
        try
        {
            respuesta = await llamada();
        }
        catch (HttpRequestException ex)
        {
            throw new ServicioException(
                "No se pudo conectar con el servidor. Verifique que el backend esté en ejecución.", null, ex);
        }
        catch (TaskCanceledException ex)
        {
            // HttpClient.Timeout vence con TaskCanceledException
            throw new ServicioException("El servidor tardó demasiado en responder. Intente nuevamente.", null, ex);
        }

        if (respuesta.IsSuccessStatusCode)
        {
            return respuesta;
        }

        var codigo = (int)respuesta.StatusCode;
        var mensaje = await LeerMensajeDeError(respuesta);
        respuesta.Dispose();
        throw new ServicioException(mensaje, codigo);
    }

    private static async Task<TResponse> Leer<TResponse>(HttpResponseMessage respuesta)
    {
        try
        {
            var valor = await respuesta.Content.ReadFromJsonAsync<TResponse>();
            return valor ?? throw new ServicioException("El servidor devolvió una respuesta vacía.", (int)respuesta.StatusCode);
        }
        catch (JsonException ex)
        {
            throw new ServicioException("La respuesta del servidor no tiene el formato esperado.", (int)respuesta.StatusCode, ex);
        }
        catch (NotSupportedException ex)
        {
            throw new ServicioException("La respuesta del servidor no es JSON.", (int)respuesta.StatusCode, ex);
        }
    }

    /// <summary>El backend responde {"error": "mensaje"}; si no viene así se usa un texto según el código.</summary>
    private static async Task<string> LeerMensajeDeError(HttpResponseMessage respuesta)
    {
        try
        {
            var cuerpo = await respuesta.Content.ReadAsStringAsync();
            if (!string.IsNullOrWhiteSpace(cuerpo))
            {
                using var json = JsonDocument.Parse(cuerpo);
                if (json.RootElement.ValueKind == JsonValueKind.Object
                    && json.RootElement.TryGetProperty("error", out var error)
                    && error.ValueKind == JsonValueKind.String
                    && !string.IsNullOrWhiteSpace(error.GetString()))
                {
                    return error.GetString()!;
                }
            }
        }
        catch (JsonException)
        {
            // cuerpo que no es JSON: se usa el texto por código
        }

        return respuesta.StatusCode switch
        {
            HttpStatusCode.BadRequest => "La solicitud no es válida.",
            HttpStatusCode.NotFound => "No se encontró lo solicitado.",
            HttpStatusCode.Unauthorized => "Credenciales o sesión no válidas.",
            _ when (int)respuesta.StatusCode >= 500 => "Ocurrió un error en el servidor. Intente nuevamente.",
            _ => $"El servidor respondió con el código {(int)respuesta.StatusCode}."
        };
    }
}
