using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace KlamWA.Services;

// Lectura común de respuestas del REST Java: éxito -> JSON; error -> {"error": "mensaje"}.
// Si hay sesión iniciada, cada llamada lleva su token (Authorization: Bearer ...): el servidor verifica identidad y rol.
public abstract class KlamApiBase
{
    protected readonly HttpClient Http;

    protected KlamApiBase(HttpClient http, SesionUsuario? sesion = null)
    {
        Http = http;
        if (sesion?.Token is { Length: > 0 } token)
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    protected static async Task<ResultadoApi<T>> LeerAsync<T>(HttpResponseMessage respuesta)
    {
        if (respuesta.IsSuccessStatusCode)
        {
            var datos = await respuesta.Content.ReadFromJsonAsync<T>();
            return datos is null
                ? ResultadoApi<T>.Falla("El servidor devolvió una respuesta vacía")
                : ResultadoApi<T>.Exito(datos);
        }
        return ResultadoApi<T>.Falla(await LeerErrorAsync(respuesta));
    }

    // Para respuestas 204 sin cuerpo (modificaciones).
    protected static async Task<ResultadoApi<bool>> LeerSinCuerpoAsync(HttpResponseMessage respuesta) =>
        respuesta.IsSuccessStatusCode
            ? ResultadoApi<bool>.Exito(true)
            : ResultadoApi<bool>.Falla(await LeerErrorAsync(respuesta));

    private static async Task<string> LeerErrorAsync(HttpResponseMessage respuesta)
    {
        try
        {
            var cuerpo = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, JsonElement>>();
            if (cuerpo is not null && cuerpo.TryGetValue("error", out var msg) && msg.ValueKind == JsonValueKind.String)
                return msg.GetString()!;
        }
        catch (JsonException) { /* cuerpo que no es JSON: se informa el código HTTP */ }
        return respuesta.StatusCode switch
        {
            System.Net.HttpStatusCode.Unauthorized => "Su sesión no es válida o expiró. Vuelva a iniciar sesión.",
            System.Net.HttpStatusCode.Forbidden => "Su perfil no tiene permiso para realizar esta acción.",
            _ => $"El servidor respondió {(int)respuesta.StatusCode} {respuesta.ReasonPhrase}"
        };
    }

    protected static ResultadoApi<T> SinConexion<T>() =>
        ResultadoApi<T>.Falla("No se pudo conectar con el servidor. Verifique que el REST Java esté en ejecución.");
}
