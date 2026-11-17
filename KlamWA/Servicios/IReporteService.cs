using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface IReporteService
{
    /// RF18: uso de insumos con los criterios dados (vista previa).
    Task<ReporteUsoInsumosDTO> UsoInsumosAsync(FiltroUsoInsumos filtro);

    /// RF18: mismo reporte en "excel" o "pdf".
    Task<ArchivoReporteDTO> ExportarUsoInsumosAsync(FiltroUsoInsumos filtro, string formato);

    /// RF19: cirugías con consumo completo que cumplen los criterios (vista previa).
    Task<ReporteComparativoDTO> ComparativoAsync(FiltroComparativo filtro);

    /// RF19: mismo reporte en "excel" o "pdf".
    Task<ArchivoReporteDTO> ExportarComparativoAsync(FiltroComparativo filtro, string formato);

    /// Catálogo de consumibles activos, para los filtros de RF18.
    Task<List<ConsumibleDTO>> ListarConsumiblesAsync();
}