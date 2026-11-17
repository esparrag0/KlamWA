using KlamWA.DTOs;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class UsuarioService : IUsuarioService
{
    private readonly RestConSesion _rest;
    private readonly string _baseUrl;

    public UsuarioService(IConfiguration configuracion, RestConSesion rest)
    {
        _rest = rest;
        _baseUrl = $"{configuracion["RestApi:BaseUrl"]!.TrimEnd('/')}/usuarios";
    }

    public async Task<List<UsuarioDTO>> ListarAsync(string? rol = null, string? estado = null, string? busqueda = null)
    {
        var query = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(rol)) query["rol"] = rol;
        if (!string.IsNullOrWhiteSpace(estado)) query["estado"] = estado;
        if (!string.IsNullOrWhiteSpace(busqueda)) query["busqueda"] = busqueda;

        string url = query.Count > 0 ? QueryHelpers.AddQueryString(_baseUrl, query) : _baseUrl;
        return await _rest.DoGet<List<UsuarioDTO>>(url);
    }

    public async Task<MetricasUsuariosDTO> MetricasAsync()
    {
        string url = $"{_baseUrl}/metricas";
        return await _rest.DoGet<MetricasUsuariosDTO>(url);
    }

    public async Task<List<UsuarioDTO>> ListarInstrumentistasActivosAsync()
    {
        string url = $"{_baseUrl}/instrumentistas-activos";
        return await _rest.DoGet<List<UsuarioDTO>>(url);
    }

    public async Task<UsuarioDTO?> ObtenerPorIdAsync(int id, string? rol = null)
    {
        string url = $"{_baseUrl}/{id}";
        if (!string.IsNullOrWhiteSpace(rol))
        {
            url = QueryHelpers.AddQueryString(url, "rol", rol);
        }
        try
        {
            return await _rest.DoGet<UsuarioDTO>(url);
        }
        catch (ServicioException ex) when (ex.CodigoHttp == 404)
        {
            return null;
        }
    }

    public async Task<UsuarioDTO> CrearAsync(UsuarioFormDTO dto)
    {
        return await _rest.DoPost<UsuarioFormDTO, UsuarioDTO>(_baseUrl, dto);
    }

    public async Task<UsuarioDTO> ModificarAsync(int id, UsuarioFormDTO dto, string? rol = null)
    {
        string url = $"{_baseUrl}/{id}";
        if (!string.IsNullOrWhiteSpace(rol))
        {
            url = QueryHelpers.AddQueryString(url, "rol", rol);
        }
        return await _rest.DoPut<UsuarioFormDTO, UsuarioDTO>(url, dto);
    }

    public async Task EliminarAsync(int id, string? rol = null)
    {
        string url = $"{_baseUrl}/{id}";
        if (!string.IsNullOrWhiteSpace(rol))
        {
            url = QueryHelpers.AddQueryString(url, "rol", rol);
        }
        await _rest.DoDelete(url);
    }

    public async Task ReactivarAsync(int id, string? rol = null)
    {
        string url = $"{_baseUrl}/{id}/reactivar";
        if (!string.IsNullOrWhiteSpace(rol))
        {
            url = QueryHelpers.AddQueryString(url, "rol", rol);
        }
        await _rest.DoPost(url);
    }
}
