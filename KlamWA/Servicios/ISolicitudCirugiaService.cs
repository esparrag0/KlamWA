using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ISolicitudCirugiaService
{
    Task<List<SolicitudCirugiaDTO>> ListarAsync();
    Task<List<SolicitudCirugiaDTO>> ListarMiasAsync();
    Task<List<string>> ListarProcedimientosAsync();
    Task<SolicitudCirugiaDTO> RegistrarAsync(RegistrarSolicitudDTO solicitud);
    Task<SolicitudCirugiaDTO> ConsultarAsync(int idSolicitud);
    Task AprobarAsync(int idSolicitud);
    Task<int> RegistrarCirugiaDesdeSolicitudAsync(int idSolicitud);
    Task RechazarAsync(int idSolicitud, string motivo);
}
