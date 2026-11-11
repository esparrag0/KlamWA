namespace KlamWA.DTOs;

/// <summary>Fila del historial de un cliente (RF10). Refleja las claves camelCase de CirugiaBL.buscarDetalle.</summary>
public sealed class CirugiaHistorialDTO
{
    public int IdCirugia { get; set; }
    public DateTime FechaHoraInicio { get; set; }
    public DateTime? FechaHoraFin { get; set; }
    public string TipoProcedimiento { get; set; } = "";
    public string DoctorNombre { get; set; } = "";
    public string? MotivoCancelacion { get; set; }

    /// <summary>Valor interno de la BD: FINALIZADA o CANCELADA.</summary>
    public string Estado { get; set; } = "";
    public string TipoCliente { get; set; } = "";
    public int IdCliente { get; set; }
    public string? ClienteNombre { get; set; }
}