using System.Net;
using KlamWA.DTOs;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class NotificacionService : INotificacionService
{
    private string BaseUrl { get; set; }

    public NotificacionService(IConfiguration configuracion)
    {
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/notificaciones";
    }

    public async Task<List<NotificacionDTO>> ListarAsync(string? rol = null, string? estado = null)
    {
        var query = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(rol)) query["rol"] = rol;
        if (!string.IsNullOrWhiteSpace(estado)) query["estado"] = estado;

        string url = query.Count > 0 ? QueryHelpers.AddQueryString(BaseUrl, query) : BaseUrl;
        return await HttpUtils.DoGet<List<NotificacionDTO>>(url);
    }

    public async Task<NotificacionResumenDTO> ResumenAsync(string? rol = null)
    {
        string url = $"{BaseUrl}/resumen";
        if (!string.IsNullOrWhiteSpace(rol))
        {
            url = QueryHelpers.AddQueryString(url, "rol", rol);
        }
        return await HttpUtils.DoGet<NotificacionResumenDTO>(url);
    }

    public async Task<NotificacionDTO?> ObtenerAsync(int id)
    {
        string url = $"{BaseUrl}/{id}";
        try
        {
            return await HttpUtils.DoGet<NotificacionDTO>(url);
        }
        catch (ServicioException ex) when (ex.EsNoEncontrado)
        {
            return null;
        }
    }

    public async Task MarcarLeidaAsync(int id)
    {
        string url = $"{BaseUrl}/{id}/lectura";
        await HttpUtils.DoPost(url);
    }

    public async Task<int> MarcarTodasLeidasAsync(string? rol = null)
    {
        string url = $"{BaseUrl}/lectura";
        if (!string.IsNullOrWhiteSpace(rol))
        {
            url = QueryHelpers.AddQueryString(url, "rol", rol);
        }
        var res = await HttpUtils.DoPost<object, Dictionary<string, int>>(url, new { });
        return res != null && res.TryGetValue("marcadas", out int marcadas) ? marcadas : 0;
    }
}
