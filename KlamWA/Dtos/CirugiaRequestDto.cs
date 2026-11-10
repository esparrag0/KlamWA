namespace KlamWA.Dtos;

// RF09: cuerpo de POST cirugias (Java: CirugiaRequestDTO). Se registra siempre PROGRAMADA.
// El responsable lo toma el servidor de la sesión (token); no viaja aquí.
public class CirugiaRequestDto
{
    public string TipoCliente { get; set; } = "";   // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    public int? IdCliente { get; set; }
    public string? Dni { get; set; }
    public string Diagnostico { get; set; } = "";
    public string TipoProcedimiento { get; set; } = "";
    public string DoctorNombre { get; set; } = "";
    public string FechaHoraInicio { get; set; } = "";
    public string? FechaHoraFin { get; set; }
    public int? IdEquipo { get; set; }
}
