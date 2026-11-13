using KlamWA.DTOs;
using KlamWA.ViewModels;

namespace KlamWA.Servicios;

public interface ICirugiaService
{
    // Consultas (rol 1: RF06, RF17, RF10)
    Task<List<CirugiaResumenViewModel>> Buscar(FiltroCirugia? filtro = null);

    Task<List<CirugiaResumenViewModel>> ObtenerCalendario(int anio, int mes);

    Task<List<CirugiaResumenViewModel>> HistorialDeCliente(string tipoCliente, int idCliente);

    Task<CirugiaDetalleViewModel?> FindById(int id);

    // Gestión (rol 5: RF02)
    Task<CirugiaDTO?> ObtenerAsync(int id);
    Task<OpcionesCirugiaDTO> OpcionesAsync(string? procedimiento = null);
    Task<CirugiaDTO> RegistrarAsync(CirugiaFormDTO cirugia);
    Task<CirugiaDTO> ModificarAsync(int id, CirugiaFormDTO cirugia);
    Task<CirugiaDTO> CambiarEstadoAsync(int id, string estado);
    Task<CirugiaDTO> CancelarAsync(int id, string motivo);
    Task EliminarAsync(int id);
}
