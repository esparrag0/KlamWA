using KlamWA.DTOs;
using KlamWA.ViewModels;

namespace KlamWA.Servicios;

public interface IClienteService
{
    Task<List<ClienteOpcionViewModel>> Listar(string? tipoCliente = null);
    Task<List<ClienteBusquedaDTO>> BuscarParticularAsync(string dni);
    Task<List<ClienteBusquedaDTO>> BuscarClinicasPorRucAsync(string ruc);
    Task<List<ClienteBusquedaDTO>> BuscarClinicasPorNombreAsync(string nombre);
    Task<ClienteBusquedaDTO> RegistrarParticularAsync(ClienteRegistroDTO cliente);
    Task<ClienteBusquedaDTO> RegistrarClinicaAsync(ClienteRegistroDTO cliente);
}
