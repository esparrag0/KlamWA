using KlamWA.DTOs;
using KlamWA.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class ClienteService : IClienteService
{
    private string BaseUrl { get; set; }

    public ClienteService(IConfiguration configuracion)
    {
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/clientes";
    }

    public async Task<List<ClienteOpcionViewModel>> Listar(string? tipoCliente = null)
    {
        string url = string.IsNullOrWhiteSpace(tipoCliente) ? BaseUrl :
            QueryHelpers.AddQueryString(BaseUrl, "tipoCliente", tipoCliente);
        var datos = await HttpUtils.DoGet<List<ClienteDTO>>(url) ?? [];
        return datos.Select(CirugiaMapeo.ToOpcion).ToList();
    }

    public async Task<List<ClienteBusquedaDTO>> BuscarParticularAsync(string dni) =>
        await HttpUtils.DoGet<List<ClienteBusquedaDTO>>(
            $"{BaseUrl}/particulares?dni={Uri.EscapeDataString(dni)}") ?? [];

    public async Task<List<ClienteBusquedaDTO>> BuscarClinicasPorRucAsync(string ruc) =>
        await HttpUtils.DoGet<List<ClienteBusquedaDTO>>(
            $"{BaseUrl}/clinicas?ruc={Uri.EscapeDataString(ruc)}") ?? [];

    public async Task<List<ClienteBusquedaDTO>> BuscarClinicasPorNombreAsync(string nombre) =>
        await HttpUtils.DoGet<List<ClienteBusquedaDTO>>(
            $"{BaseUrl}/clinicas?nombre={Uri.EscapeDataString(nombre)}") ?? [];

    public async Task<ClienteBusquedaDTO> RegistrarParticularAsync(ClienteRegistroDTO cliente) =>
        await HttpUtils.DoPost<ClienteRegistroDTO, ClienteBusquedaDTO>($"{BaseUrl}/particulares", cliente);

    public async Task<ClienteBusquedaDTO> RegistrarClinicaAsync(ClienteRegistroDTO cliente) =>
        await HttpUtils.DoPost<ClienteRegistroDTO, ClienteBusquedaDTO>($"{BaseUrl}/clinicas", cliente);
}
