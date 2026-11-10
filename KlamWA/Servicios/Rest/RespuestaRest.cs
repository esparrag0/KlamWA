using System.Net;
using System.Net.Http.Json;

namespace KlamWA.Servicios.Rest;

// Convierte las respuestas de error del backend ({"error": "..."}) en ServicioException
public static class RespuestaRest
{
    public static async Task<T> LeerAsync<T>(HttpResponseMessage respuesta)
    {
        await VerificarAsync(respuesta);
        T? cuerpo = await respuesta.Content.ReadFromJsonAsync<T>();
        return cuerpo ?? throw new ServicioException("El servicio no devolvió datos");
    }

    public static async Task VerificarAsync(HttpResponseMessage respuesta)
    {
        if (respuesta.IsSuccessStatusCode)
        {
            return;
        }
        string mensaje = respuesta.StatusCode switch
        {
            HttpStatusCode.Unauthorized => "Su sesión no es válida",
            HttpStatusCode.Forbidden => "No tiene permiso para esta acción",
            HttpStatusCode.NotFound => "No se encontró el registro",
            _ => $"Error del servicio ({(int)respuesta.StatusCode})"
        };
        try
        {
            Dictionary<string, string>? error = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            if (error != null && error.TryGetValue("error", out string? texto) && !string.IsNullOrWhiteSpace(texto))
            {
                mensaje = texto;
            }
        }
        catch (Exception)
        {
            // el cuerpo no era JSON: se queda el mensaje genérico
        }
        throw new ServicioException(mensaje);
    }
}
