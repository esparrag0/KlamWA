namespace KlamWA.DTOs;

// RF09: cuerpo de POST admin/cirugias (Java: RegistroCirugiaAdminDTO). Se registra siempre en estado PROGRAMADA.
public sealed class CirugiaRegistroDTO
{
    public string FechaHoraInicio { get; set; } = string.Empty;   // ISO-8601, ej. 2026-11-20T08:00
    public string? FechaHoraFin { get; set; }
    public string TipoProcedimiento { get; set; } = string.Empty;
    public string DoctorNombre { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public string TipoCliente { get; set; } = string.Empty;       // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    public int IdCliente { get; set; }
    public int? IdEquipo { get; set; }
}

// Respuesta de POST admin/cirugias: idCirugia y estado.
public sealed class CirugiaRegistradaDTO
{
    public int IdCirugia { get; set; }
    public string? Estado { get; set; }
}
