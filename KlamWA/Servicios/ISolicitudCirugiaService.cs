using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ISolicitudCirugiaService
{
    Task<List<SolicitudCirugiaDTO>> ListarAsync();
    Task<List<SolicitudCirugiaDTO>> ListarMiasAsync();
    Task<SolicitudCirugiaDTO> RegistrarAsync(RegistrarSolicitudDTO solicitud);
    Task<SolicitudCirugiaDTO> ConsultarAsync(int idSolicitud);
    Task AprobarAsync(int idSolicitud);
    Task RechazarAsync(int idSolicitud, string motivo);
}
