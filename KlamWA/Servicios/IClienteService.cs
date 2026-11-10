using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface IClienteService
{
    Task<List<ClienteBusquedaDTO>> BuscarParticularAsync(string dni);
    Task<List<ClienteBusquedaDTO>> BuscarClinicasPorRucAsync(string ruc);
    Task<List<ClienteBusquedaDTO>> BuscarClinicasPorNombreAsync(string nombre);
    Task<ClienteBusquedaDTO> RegistrarParticularAsync(ClienteRegistroDTO cliente);
    Task<ClienteBusquedaDTO> RegistrarClinicaAsync(ClienteRegistroDTO cliente);
}
