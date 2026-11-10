using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface INotificacionService
{
    Task<List<NotificacionDTO>> ListarAsync(string? rol = null, string? estado = null);
    Task<NotificacionResumenDTO> ResumenAsync(string? rol = null);
    Task<NotificacionDTO?> ObtenerAsync(int id);
    Task MarcarLeidaAsync(int id);
    Task<int> MarcarTodasLeidasAsync(string? rol = null);
}
