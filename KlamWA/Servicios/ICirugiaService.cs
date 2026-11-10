using KlamWA.ViewModels;

namespace KlamWA.Servicios;

public interface ICirugiaService
{
    Task<List<CirugiaResumenViewModel>> Buscar(FiltroCirugia? filtro = null);

    Task<List<CirugiaResumenViewModel>> ObtenerCalendario(int anio, int mes);

    Task<List<CirugiaResumenViewModel>> HistorialDeCliente(string tipoCliente, int idCliente);

    Task<CirugiaDetalleViewModel?> FindById(int id);
}
