namespace KlamWA.DTOs;

public sealed class RegistrarSolicitudDTO
{
    public string TipoCliente { get; set; } = string.Empty;
    public int IdCliente { get; set; }
    public int IdClinicaDestino { get; set; }
    public int IdVendedor { get; set; }
    public string PacienteNombre { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public string TipoProcedimiento { get; set; } = string.Empty;
    public string DoctorNombre { get; set; } = string.Empty;
    public string FechaHoraSolicitada { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}
