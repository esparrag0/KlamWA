using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class RestConSesion(HttpClient client, AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens)
{
    private async Task<T?> Enviar<T>(HttpMethod metodo, string url, object? datos = null)
    {
        using var peticion = new HttpRequestMessage(metodo, url);
        peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await tokens.ObtenerAsync(autenticacion));
        if (datos != null) peticion.Content = JsonContent.Create(datos);
        using var respuesta = await client.SendAsync(peticion);
        if (!respuesta.IsSuccessStatusCode)
        {
            string mensaje = $"El servidor respondió {(int)respuesta.StatusCode}.";
            try {
                using var json = JsonDocument.Parse(await respuesta.Content.ReadAsStringAsync());
                if (json.RootElement.TryGetProperty("error", out var error)) mensaje = error.GetString() ?? mensaje;
            } catch (JsonException) { }
            throw new ServicioException(mensaje, (int)respuesta.StatusCode);
        }
        if (typeof(T) == typeof(object) || respuesta.Content.Headers.ContentLength == 0) return default;
        return await respuesta.Content.ReadFromJsonAsync<T>();
    }

    public async Task<T> DoGet<T>(string url) => await Enviar<T>(HttpMethod.Get, url)
        ?? throw new ServicioException("Respuesta vacía", 500);
    public async Task<T> DoPost<TRequest, T>(string url, TRequest datos) =>
        await Enviar<T>(HttpMethod.Post, url, datos) ?? throw new ServicioException("Respuesta vacía", 500);
    public Task DoPost<TRequest>(string url, TRequest datos) => Enviar<object>(HttpMethod.Post, url, datos);
    public Task DoPost(string url) => Enviar<object>(HttpMethod.Post, url);
    public async Task<T> DoPut<TRequest, T>(string url, TRequest datos) =>
        await Enviar<T>(HttpMethod.Put, url, datos) ?? throw new ServicioException("Respuesta vacía", 500);
    public Task DoPut<TRequest>(string url, TRequest datos) => Enviar<object>(HttpMethod.Put, url, datos);
    public Task DoDelete(string url) => Enviar<object>(HttpMethod.Delete, url);
}
