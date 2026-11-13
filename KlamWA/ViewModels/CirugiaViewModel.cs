using System.ComponentModel.DataAnnotations;
using KlamWA.DTOs;

namespace KlamWA.ViewModels;

// Formulario de cirugía (RF02). Las anotaciones solo ayudan al usuario;
// las reglas reales (cruces de horario, esterilización, estados) las valida CirugiaBL en Java.
public class CirugiaViewModel : IValidatableObject
{
    [Required(ErrorMessage = "El tipo de procedimiento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string TipoProcedimiento { get; set; } = "";

    [Required(ErrorMessage = "El nombre del doctor es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Doctor { get; set; } = "";

    [Required(ErrorMessage = "El diagnóstico es obligatorio")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string Diagnostico { get; set; } = "";

    [Required(ErrorMessage = "La fecha y hora de inicio es obligatoria")]
    public DateTime? FechaHoraInicio { get; set; }

    public DateTime? FechaHoraFin { get; set; }

    // "TIPO:id" porque los ids de clínicas y pacientes se repiten (CLINICA_HOSPITAL:2)
    [Required(ErrorMessage = "Debe seleccionar un cliente")]
    public string ClienteClave { get; set; } = "";

    public int IdEquipo { get; set; }    // 0 = ninguno
    public int IdBandeja { get; set; }   // 0 = ninguna
    public Dictionary<int, int> ConsumiblesPrevistos { get; set; } = [];

    public CirugiaViewModel() { }

    public CirugiaViewModel(CirugiaDTO c)
    {
        TipoProcedimiento = c.TipoProcedimiento;
        Doctor = c.DoctorNombre;
        Diagnostico = c.Diagnostico;
        FechaHoraInicio = c.Inicio;
        FechaHoraFin = c.Fin;
        ClienteClave = c.IdCliente is int id ? $"{c.TipoCliente}:{id}" : "";
        IdEquipo = c.IdEquipo ?? 0;
        IdBandeja = c.IdBandeja ?? 0;
        ConsumiblesPrevistos = new(c.ConsumiblesPrevistos);
    }

    public CirugiaFormDTO ToFormDTO()
    {
        string[] partes = ClienteClave.Split(':');
        return new CirugiaFormDTO
        {
            FechaHoraInicio = FechaHoraInicio?.ToString("yyyy-MM-dd'T'HH:mm:ss"),
            FechaHoraFin = FechaHoraFin?.ToString("yyyy-MM-dd'T'HH:mm:ss"),
            TipoProcedimiento = TipoProcedimiento,
            DoctorNombre = Doctor,
            Diagnostico = Diagnostico,
            TipoCliente = partes.Length == 2 ? partes[0] : "",
            IdCliente = partes.Length == 2 && int.TryParse(partes[1], out int id) ? id : 0,
            IdEquipo = IdEquipo > 0 ? IdEquipo : null,
            IdBandeja = IdBandeja > 0 ? IdBandeja : null
            ,ConsumiblesPrevistos = new(ConsumiblesPrevistos)
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (FechaHoraInicio != null && FechaHoraFin != null && FechaHoraInicio >= FechaHoraFin)
            yield return new ValidationResult(
                "La fecha y hora de inicio debe ser anterior a la de fin",
                new[] { nameof(FechaHoraFin) });
    }
}
