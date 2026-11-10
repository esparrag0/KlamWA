using System.Net;
using KlamWA.DTOs;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class DespachoService : IDespachoService
{
    private string BaseUrl { get; set; }

    public DespachoService(IConfiguration configuracion)
    {
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/despachos";
    }

    public async Task<List<DespachoDTO>> ListarAsync(string? estado = null, string? busqueda = null)
    {
        var query = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(estado)) query["estado"] = estado;
        if (!string.IsNullOrWhiteSpace(busqueda)) query["q"] = busqueda;

        string url = query.Count > 0 ? QueryHelpers.AddQueryString(BaseUrl, query) : BaseUrl;
        return await HttpUtils.DoGet<List<DespachoDTO>>(url);
    }

    public async Task<List<CirugiaDespachableDTO>> CirugiasDespachablesAsync()
    {
        string url = $"{BaseUrl}/cirugias-despachables";
        return await HttpUtils.DoGet<List<CirugiaDespachableDTO>>(url);
    }

    public async Task<DespachoDetalleDTO?> ObtenerAsync(string codigo)
    {
        string url = $"{BaseUrl}/{codigo}";
        try
        {
            return await HttpUtils.DoGet<DespachoDetalleDTO>(url);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<string> RegistrarAsync(NuevoDespachoDTO despacho)
    {
        string url = BaseUrl;
        var res = await HttpUtils.DoPost<NuevoDespachoDTO, DespachoRespuestaDTO>(url, despacho);
        return res?.Codigo ?? "";
    }

    public async Task RegistrarRetornoAsync(string codigo)
    {
        string url = $"{BaseUrl}/{codigo}/retorno";
        await HttpUtils.DoPost(url);
    }
}
