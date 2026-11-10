namespace KlamWA.Dtos;

// RF05: cuerpo de POST clientes/{tipo}/{id}/solicitudes (Java: SolicitudCirugiaRequestDTO).
// Los nombres se serializan en camelCase y deben coincidir con los del DTO de Java.
public class SolicitudCirugiaRequestDto
{
    public int? IdClinicaDestino { get; set; }
    public string? PacienteNombre { get; set; }
    public string? Dni { get; set; }
    public string Diagnostico { get; set; } = "";
    public string TipoProcedimiento { get; set; } = "";
    public string DoctorNombre { get; set; } = "";
    // ISO-8601 sin zona: "2026-11-12T09:30:00"
    public string FechaHoraSolicitada { get; set; } = "";
    public string? Observaciones { get; set; }
}
