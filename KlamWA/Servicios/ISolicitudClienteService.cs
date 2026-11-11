using KlamWA.DTOs;

namespace KlamWA.Servicios;

// RF05: el cliente registra y consulta SUS solicitudes. El solicitante lo determina el servidor por la sesión.
public interface ISolicitudClienteService
{
    Task<SolicitudCirugiaDTO> RegistrarAsync(RegistrarSolicitudDTO solicitud);
    Task<List<SolicitudCirugiaDTO>> ListarPropiasAsync();

    // Buscador de clínicas por nombre (el cliente no conoce los ids): devuelve hasta 10 coincidencias.
    Task<List<ClinicaOpcionDTO>> BuscarClinicasAsync(string texto);
}
