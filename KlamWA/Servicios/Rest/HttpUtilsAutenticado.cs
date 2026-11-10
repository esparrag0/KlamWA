using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KlamWA.Servicios.Rest;

// Complemento de HttpUtils (que tiene DoGetAutenticado/DoPostAutenticado): agrega PUT y un GET/POST con la misma forma.
// Los errores los traduce HttpUtils.VerificarRespuestaAsync a HttpRequestException con el mensaje del servidor.
public static class HttpUtilsAutenticado
{
    public static async Task<TRespuesta?> DoGet<TRespuesta>(HttpClient client, string ruta, string token)
    {
        using var request = Crear(HttpMethod.Get, ruta, token);
        using var response = await client.SendAsync(request);
        await HttpUtils.VerificarRespuestaAsync(response);
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task<TRespuesta?> DoPost<TSolicitud, TRespuesta>(
        HttpClient client, string ruta, string token, TSolicitud datos)
    {
        using var request = Crear(HttpMethod.Post, ruta, token);
        request.Content = JsonContent.Create(datos);
        using var response = await client.SendAsync(request);
        await HttpUtils.VerificarRespuestaAsync(response);
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }

    public static async Task DoPut<TSolicitud>(HttpClient client, string ruta, string token, TSolicitud datos)
    {
        using var request = Crear(HttpMethod.Put, ruta, token);
        request.Content = JsonContent.Create(datos);
        using var response = await client.SendAsync(request);
        await HttpUtils.VerificarRespuestaAsync(response);
    }

    private static HttpRequestMessage Crear(HttpMethod metodo, string ruta, string token)
    {
        var request = new HttpRequestMessage(metodo, ruta);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }
}
