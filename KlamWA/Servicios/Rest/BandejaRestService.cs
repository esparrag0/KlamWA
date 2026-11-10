using System.Net;
using System.Net.Http.Json;
using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// Consume BandejasResource y ConsumiblesResource (RF13) del backend Java
public class BandejaRestService : IBandejaService
{
    private readonly HttpClient http;

    public BandejaRestService(HttpClient http)
    {
        this.http = http;
    }

    public async Task<List<BandejaDTO>> ListarAsync(string? procedimiento)
    {
        string ruta = string.IsNullOrWhiteSpace(procedimiento)
            ? "bandejas"
            : $"bandejas?procedimiento={Uri.EscapeDataString(procedimiento)}";
        using HttpResponseMessage respuesta = await http.GetAsync(ruta);
        return await RespuestaRest.LeerAsync<List<BandejaDTO>>(respuesta);
    }

    public async Task<BandejaDTO?> ObtenerAsync(int id)
    {
        using HttpResponseMessage respuesta = await http.GetAsync($"bandejas/{id}");
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        return await RespuestaRest.LeerAsync<BandejaDTO>(respuesta);
    }

    public async Task<BandejaDTO> RegistrarAsync(BandejaFormDTO bandeja)
    {
        using HttpResponseMessage respuesta = await http.PostAsJsonAsync("bandejas", bandeja);
        return await RespuestaRest.LeerAsync<BandejaDTO>(respuesta);
    }

    public async Task<BandejaDTO> AsociarConsumibleAsync(int idBandeja, int idConsumible, int cantidad)
    {
        using HttpResponseMessage respuesta = await http.PostAsJsonAsync($"bandejas/{idBandeja}/consumibles",
            new ConsumibleBandejaDTO { IdConsumible = idConsumible, Cantidad = cantidad });
        return await RespuestaRest.LeerAsync<BandejaDTO>(respuesta);
    }

    public async Task<BandejaDTO> ActualizarCantidadAsync(int idBandeja, int idConsumible, int cantidad)
    {
        using HttpResponseMessage respuesta = await http.PutAsJsonAsync($"bandejas/{idBandeja}/consumibles/{idConsumible}",
            new ConsumibleBandejaDTO { IdConsumible = idConsumible, Cantidad = cantidad });
        return await RespuestaRest.LeerAsync<BandejaDTO>(respuesta);
    }

    public async Task<BandejaDTO> QuitarConsumibleAsync(int idBandeja, int idConsumible)
    {
        using HttpResponseMessage respuesta = await http.DeleteAsync($"bandejas/{idBandeja}/consumibles/{idConsumible}");
        return await RespuestaRest.LeerAsync<BandejaDTO>(respuesta);
    }

    public async Task<List<ConsumibleDTO>> ListarConsumiblesAsync()
    {
        using HttpResponseMessage respuesta = await http.GetAsync("consumibles");
        return await RespuestaRest.LeerAsync<List<ConsumibleDTO>>(respuesta);
    }
}
