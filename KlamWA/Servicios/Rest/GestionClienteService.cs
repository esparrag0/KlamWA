using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class GestionClienteService(
    HttpClient client,
    AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : IGestionClienteService
{
    public async Task<List<ClienteGestionDTO>> ListarAsync(string? tipoCliente = null)
    {
        var ruta = string.IsNullOrWhiteSpace(tipoCliente)
            ? "gestion-clientes"
            : $"gestion-clientes?tipo={Uri.EscapeDataString(tipoCliente)}";
        return await HttpUtilsAutenticado.DoGet<List<ClienteGestionDTO>>(
            client, ruta, await tokens.ObtenerAsync(autenticacion)) ?? [];
    }

    public async Task<ClienteGestionDTO> RegistrarAsync(ClienteFormDTO cliente) =>
        await HttpUtilsAutenticado.DoPost<ClienteFormDTO, ClienteGestionDTO>(
            client, "gestion-clientes", await tokens.ObtenerAsync(autenticacion), cliente)
            ?? throw new HttpRequestException("La respuesta del alta de cliente está vacía.");

    public async Task<ClienteFormDTO> ObtenerAsync(string tipoCliente, int id) =>
        await HttpUtilsAutenticado.DoGet<ClienteFormDTO>(
            client, $"gestion-clientes/{Uri.EscapeDataString(tipoCliente)}/{id}",
            await tokens.ObtenerAsync(autenticacion))
            ?? throw new HttpRequestException("La respuesta del cliente está vacía.");

    public async Task ActualizarAsync(string tipoCliente, int id, ClienteFormDTO cliente) =>
        await HttpUtilsAutenticado.DoPut(
            client, $"gestion-clientes/{Uri.EscapeDataString(tipoCliente)}/{id}",
            await tokens.ObtenerAsync(autenticacion), cliente);
}
