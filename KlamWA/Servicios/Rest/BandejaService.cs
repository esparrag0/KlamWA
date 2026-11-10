using System.Net;
using KlamWA.DTOs;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

// Consume BandejasResource y ConsumiblesResource (RF13) del backend Java
public class BandejaService : IBandejaService
{
    private string BaseUrlBandejas { get; set; }
    private string BaseUrlConsumibles { get; set; }

    public BandejaService(IConfiguration configuracion)
    {
        BaseUrlBandejas = $"{configuracion["RestApi:BaseUrl"]!}/bandejas";
        BaseUrlConsumibles = $"{configuracion["RestApi:BaseUrl"]!}/consumibles";
    }

    public async Task<List<BandejaDTO>> ListarAsync(string? procedimiento)
    {
        string url = BaseUrlBandejas;
        if (!string.IsNullOrWhiteSpace(procedimiento))
        {
            url = QueryHelpers.AddQueryString(BaseUrlBandejas, new Dictionary<string, string?> {
                { "procedimiento", procedimiento }
            });
        }
        return await HttpUtils.DoGet<List<BandejaDTO>>(url);
    }

    public async Task<BandejaDTO?> ObtenerAsync(int id)
    {
        string url = $"{BaseUrlBandejas}/{id}";
        try
        {
            return await HttpUtils.DoGet<BandejaDTO>(url);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<BandejaDTO> RegistrarAsync(BandejaFormDTO bandeja)
    {
        string url = BaseUrlBandejas;
        return await HttpUtils.DoPost<BandejaFormDTO, BandejaDTO>(url, bandeja);
    }

    public async Task<BandejaDTO> AsociarConsumibleAsync(int idBandeja, int idConsumible, int cantidad)
    {
        string url = $"{BaseUrlBandejas}/{idBandeja}/consumibles";
        return await HttpUtils.DoPost<ConsumibleBandejaDTO, BandejaDTO>(url,
            new ConsumibleBandejaDTO { IdConsumible = idConsumible, Cantidad = cantidad });
    }

    public async Task<BandejaDTO> ActualizarCantidadAsync(int idBandeja, int idConsumible, int cantidad)
    {
        string url = $"{BaseUrlBandejas}/{idBandeja}/consumibles/{idConsumible}";
        return await HttpUtils.DoPut<ConsumibleBandejaDTO, BandejaDTO>(url,
            new ConsumibleBandejaDTO { IdConsumible = idConsumible, Cantidad = cantidad });
    }

    // DELETE no devuelve cuerpo en HttpUtils: se vuelve a consultar la plantilla
    public async Task<BandejaDTO> QuitarConsumibleAsync(int idBandeja, int idConsumible)
    {
        string url = $"{BaseUrlBandejas}/{idBandeja}/consumibles/{idConsumible}";
        await HttpUtils.DoDelete(url);
        return await HttpUtils.DoGet<BandejaDTO>($"{BaseUrlBandejas}/{idBandeja}");
    }

    public async Task<List<ConsumibleDTO>> ListarConsumiblesAsync()
    {
        string url = BaseUrlConsumibles;
        return await HttpUtils.DoGet<List<ConsumibleDTO>>(url);
    }
}
