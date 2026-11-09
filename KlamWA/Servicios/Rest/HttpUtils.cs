using System.Net;
using System.Net.Http.Json;

namespace KlamWA.Servicios.Rest;

public static class HttpUtils
{
    public static async Task<TRespuesta?> DoPost<TSolicitud, TRespuesta>(
        HttpClient client, string ruta, TSolicitud datos)
    {
        using var response = await client.PostAsJsonAsync(ruta, datos);
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return default;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TRespuesta>();
    }
}
