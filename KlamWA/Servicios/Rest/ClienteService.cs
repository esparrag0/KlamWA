using KlamWA.DTOs;
using KlamWA.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class ClienteService(HttpClient client, AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : IClienteService
{
    private Task<string> TokenAsync() => tokens.ObtenerAsync(autenticacion);

    public async Task<List<ClienteOpcionViewModel>> Listar(string? tipoCliente = null)
    {
        var ruta = string.IsNullOrWhiteSpace(tipoCliente) ? "clientes" :
            QueryHelpers.AddQueryString("clientes", "tipoCliente", tipoCliente);
        var datos = await HttpUtilsAutenticado.DoGet<List<ClienteDTO>>(client, ruta, await TokenAsync()) ?? [];
        return datos.Select(CirugiaMapeo.ToOpcion).ToList();
    }

    public async Task<List<ClienteBusquedaDTO>> BuscarParticularAsync(string dni) =>
        await HttpUtils.DoGetAutenticado<List<ClienteBusquedaDTO>>(client,
            $"clientes/particulares?dni={Uri.EscapeDataString(dni)}", await TokenAsync()) ?? [];

    public async Task<List<ClienteBusquedaDTO>> BuscarClinicasPorRucAsync(string ruc) =>
        await HttpUtils.DoGetAutenticado<List<ClienteBusquedaDTO>>(client,
            $"clientes/clinicas?ruc={Uri.EscapeDataString(ruc)}", await TokenAsync()) ?? [];

    public async Task<List<ClienteBusquedaDTO>> BuscarClinicasPorNombreAsync(string nombre) =>
        await HttpUtils.DoGetAutenticado<List<ClienteBusquedaDTO>>(client,
            $"clientes/clinicas?nombre={Uri.EscapeDataString(nombre)}", await TokenAsync()) ?? [];

    public async Task<ClienteBusquedaDTO> RegistrarParticularAsync(ClienteRegistroDTO cliente) =>
        await HttpUtils.DoPostAutenticado<ClienteRegistroDTO, ClienteBusquedaDTO>(client,
            "clientes/particulares", await TokenAsync(), cliente)
            ?? throw new HttpRequestException("La respuesta del paciente está vacía.");

    public async Task<ClienteBusquedaDTO> RegistrarClinicaAsync(ClienteRegistroDTO cliente) =>
        await HttpUtils.DoPostAutenticado<ClienteRegistroDTO, ClienteBusquedaDTO>(client,
            "clientes/clinicas", await TokenAsync(), cliente)
            ?? throw new HttpRequestException("La respuesta de la clínica está vacía.");
}
