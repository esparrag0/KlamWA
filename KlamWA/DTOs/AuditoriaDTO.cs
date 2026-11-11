namespace KlamWA.DTOs;

/// <summary>Entrada de la bitacora de cirugias (RF16).</summary>
public class AuditoriaDTO
{
    public int IdAuditoria { get; set; }
    public int IdCirugia { get; set; }
    public DateTime FechaHora { get; set; }

    /// <summary>Codigo interno de la BL: CREAR, EDITAR, CANCELAR, etc.</summary>
    public string Accion { get; set; } = "";
    public string Responsable { get; set; } = "";
    public string? Detalle { get; set; }
}

/// <summary>Entrada con sus diferencias campo por campo.</summary>
public sealed class AuditoriaDetalleDTO : AuditoriaDTO
{
    public List<CambioAuditoriaDTO> Cambios { get; set; } = [];
}

/// <summary>Un campo modificado: valor anterior y posterior (null = sin valor).</summary>
public sealed class CambioAuditoriaDTO
{
    public string Campo { get; set; } = "";
    public string? ValorAnterior { get; set; }
    public string? ValorPosterior { get; set; }
}
