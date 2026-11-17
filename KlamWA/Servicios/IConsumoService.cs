using KlamWA.ViewModels;

namespace KlamWA.Servicios;

// RF15: consumo real y cierre por cirugía (solo administración).
public interface IConsumoService
{
    /// Cirugías finalizadas con el estado de su documentación de cierre y de su consumo.
    Task<List<ConsumoCirugiaViewModel>> ListarAsync(string? busqueda = null, DateTime? desde = null, DateTime? hasta = null);

    /// Detalle con despachado, consumido y diferencia por consumible; null si la cirugía no existe.
    Task<ConsumoDetalleViewModel?> ObtenerAsync(int idCirugia);

    /// Guarda las cantidades consumidas. Con confirmar = true el consumo queda completo y ya no se puede editar.
    Task<ConsumoDetalleViewModel> GuardarAsync(ConsumoDetalleViewModel consumo, bool confirmar);
}
