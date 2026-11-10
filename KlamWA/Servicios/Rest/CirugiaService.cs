using System.Net;
using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// Consume CirugiasResource (RF02) del backend Java
public class CirugiaService : ICirugiaService
{
    private string BaseUrl { get; set; }

    public CirugiaService(IConfiguration configuracion)
    {
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/cirugias";
    }

    public async Task<CirugiaDTO?> ObtenerAsync(int id)
    {
        string url = $"{BaseUrl}/{id}";
        try
        {
            return await HttpUtils.DoGet<CirugiaDTO>(url);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<OpcionesCirugiaDTO> OpcionesAsync()
    {
        string url = $"{BaseUrl}/opciones";
        return await HttpUtils.DoGet<OpcionesCirugiaDTO>(url);
    }

    public async Task<CirugiaDTO> RegistrarAsync(CirugiaFormDTO cirugia)
    {
        string url = BaseUrl;
        return await HttpUtils.DoPost<CirugiaFormDTO, CirugiaDTO>(url, cirugia);
    }

    public async Task<CirugiaDTO> ModificarAsync(int id, CirugiaFormDTO cirugia)
    {
        string url = $"{BaseUrl}/{id}";
        return await HttpUtils.DoPut<CirugiaFormDTO, CirugiaDTO>(url, cirugia);
    }

    public async Task<CirugiaDTO> CambiarEstadoAsync(int id, string estado)
    {
        string url = $"{BaseUrl}/{id}/estado";
        return await HttpUtils.DoPost<AccionCirugiaDTO, CirugiaDTO>(url, new AccionCirugiaDTO { Estado = estado });
    }

    public async Task<CirugiaDTO> CancelarAsync(int id, string motivo)
    {
        string url = $"{BaseUrl}/{id}/cancelacion";
        return await HttpUtils.DoPost<AccionCirugiaDTO, CirugiaDTO>(url, new AccionCirugiaDTO { Motivo = motivo });
    }

    public async Task EliminarAsync(int id)
    {
        string url = $"{BaseUrl}/{id}";
        await HttpUtils.DoDelete(url);
    }
}
