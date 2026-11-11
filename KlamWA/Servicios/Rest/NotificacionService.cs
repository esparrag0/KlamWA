using System.Net;
using KlamWA.DTOs;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class NotificacionService : INotificacionService
{
    private readonly ISesionService _sesion;
    private string BaseUrl { get; set; }

    public NotificacionService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/notificaciones";
    }

    // Cada usuario ve solo su bandeja: el rol que piden las pantallas se ignora y se usa el de la sesión
    public async Task<List<NotificacionDTO>> ListarAsync(string? rol = null, string? estado = null)
    {
        var query = await Usuario();
        if (!string.IsNullOrWhiteSpace(estado)) query["estado"] = estado;

        string url = QueryHelpers.AddQueryString(BaseUrl, query);
        return await HttpUtils.DoGet<List<NotificacionDTO>>(url);
    }

    public async Task<NotificacionResumenDTO> ResumenAsync(string? rol = null)
    {
        string url = QueryHelpers.AddQueryString($"{BaseUrl}/resumen", await Usuario());
        return await HttpUtils.DoGet<NotificacionResumenDTO>(url);
    }

    public async Task<NotificacionDTO?> ObtenerAsync(int id)
    {
        string url = QueryHelpers.AddQueryString($"{BaseUrl}/{id}", await Usuario());
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
        string url = QueryHelpers.AddQueryString($"{BaseUrl}/{id}/lectura", await Usuario());
        await HttpUtils.DoPost(url);
    }

    public async Task<int> MarcarTodasLeidasAsync(string? rol = null)
    {
        string url = QueryHelpers.AddQueryString($"{BaseUrl}/lectura", await Usuario());
        var res = await HttpUtils.DoPost<object, Dictionary<string, int>>(url, new { });
        return res != null && res.TryGetValue("marcadas", out int marcadas) ? marcadas : 0;
    }

    private async Task<Dictionary<string, string?>> Usuario() => new()
    {
        ["idUsuario"] = (await _sesion.ObtenerIdUsuarioAsync()).ToString(),
        ["rol"] = await _sesion.ObtenerRolAsync()
    };
}
