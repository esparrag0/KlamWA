using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface IAuditoriaService
{
    /// <summary>Todos los filtros son opcionales (null o vacio = sin filtrar).</summary>
    Task<List<AuditoriaDTO>> ListarAsync(int? idCirugia, DateTime? desde, DateTime? hasta,
                                         string? accion, string? responsable);

    /// <summary>Detalle con diferencias. Devuelve null si la entrada no existe.</summary>
    Task<AuditoriaDetalleDTO?> ObtenerAsync(int idAuditoria);
}
