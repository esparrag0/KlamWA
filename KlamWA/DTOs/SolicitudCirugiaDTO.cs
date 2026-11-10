namespace KlamWA.DTOs;

public sealed class SolicitudCirugiaDTO
{
    public int IdSolicitud { get; set; }
    public int? IdVendedorRegistrador { get; set; }
    public int IdCliente { get; set; }
    public string Cliente { get; set; } = string.Empty;
    public string TipoCliente { get; set; } = string.Empty;
    public int IdClinicaDestino { get; set; }
    public string ClinicaDestino { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string TipoProcedimiento { get; set; } = string.Empty;
    public string DoctorNombre { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public DateTime FechaHoraSolicitada { get; set; }
    public string? Observaciones { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? MotivoRechazo { get; set; }
    public DateTime? FechaSolicitud { get; set; }
    public DateTime? FechaRespuesta { get; set; }
    public string Codigo => $"SOL-{IdSolicitud:000}";
}
