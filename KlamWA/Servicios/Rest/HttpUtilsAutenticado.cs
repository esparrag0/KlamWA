using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace KlamWA.Servicios.Rest;

// Llamadas al REST con el token de la sesión (Authorization: Bearer ...): GET, POST y PUT.
// Es autosuficiente a propósito (no depende de HttpUtils): así compila con cualquiera de las versiones de HttpUtils
// que haya en el equipo. Los errores del servidor ({"error": "mensaje"}) se lanzan como HttpRequestException con
// ese mensaje, igual que el resto de servicios con token.
public static class HttpUtilsAutenticado
{
    public static async Task<TRespuesta?> DoGet<TRespuesta>(HttpClient client, string ruta, string token)
    {
        using var request = Crear(HttpMethod.Get, ruta, token);
        using var response = await client.SendAsync(request);
        await VerificarAsync(response);
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task<TRespuesta?> DoPost<TSolicitud, TRespuesta>(
        HttpClient client, string ruta, string token, TSolicitud datos)
    {
        using var request = Crear(HttpMethod.Post, ruta, token);
        request.Content = JsonContent.Create(datos);
        using var response = await client.SendAsync(request);
        await VerificarAsync(response);
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task DoPut<TSolicitud>(HttpClient client, string ruta, string token, TSolicitud datos)
    {
        using var request = Crear(HttpMethod.Put, ruta, token);
        request.Content = JsonContent.Create(datos);
        using var response = await client.SendAsync(request);
        await VerificarAsync(response);
    }

    private static HttpRequestMessage Crear(HttpMethod metodo, string ruta, string token)
    {
        var request = new HttpRequestMessage(metodo, ruta);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private static async Task VerificarAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;

        string? mensaje = null;
        try
        {
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            if (json.RootElement.ValueKind == JsonValueKind.Object
                && json.RootElement.TryGetProperty("error", out var error)
                && error.ValueKind == JsonValueKind.String)
                mensaje = error.GetString();
        }
        catch (JsonException)
        {
            // Los errores sin cuerpo JSON conservan el código HTTP.
        }

        if (string.IsNullOrWhiteSpace(mensaje))
        {
            mensaje = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "Su sesión no es válida o expiró. Vuelva a iniciar sesión.",
                HttpStatusCode.Forbidden => "Su perfil no tiene permiso para realizar esta acción.",
                _ => $"El servidor respondió {(int)response.StatusCode} ({response.ReasonPhrase})."
            };
        }
        throw new HttpRequestException(mensaje, null, response.StatusCode);
    }
}
