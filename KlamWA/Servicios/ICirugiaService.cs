using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ICirugiaService
{
    Task<CirugiaDTO?> ObtenerAsync(int id);
    Task<OpcionesCirugiaDTO> OpcionesAsync();
    Task<CirugiaDTO> RegistrarAsync(CirugiaFormDTO cirugia);
    Task<CirugiaDTO> ModificarAsync(int id, CirugiaFormDTO cirugia);
    Task<CirugiaDTO> CambiarEstadoAsync(int id, string estado);
    Task<CirugiaDTO> CancelarAsync(int id, string motivo);
    Task EliminarAsync(int id);
}
