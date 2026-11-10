using System.ComponentModel.DataAnnotations;
using KlamWA.Dtos;

namespace KlamWA.ViewModels;

// RF09: el administrador registra una cirugía (diagnóstico, fecha, hora, procedimiento, médico y clínica destino).
// Anotaciones = ayuda de UX; la validación real la hace CirugiaBL (Java) y sus mensajes se muestran tal cual.
// Si integrante 5 (RF02) prefiere un solo formulario, estos campos se pueden fusionar en CirugiaViewModel.
public class RegistroCirugiaAdminViewModel : IValidatableObject
{
    // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    [Required(ErrorMessage = "Seleccione el tipo de cliente")]
    public string TipoCliente { get; set; } = "CLINICA_HOSPITAL";

    // Con CLINICA_HOSPITAL este cliente es también la clínica destino.
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione el cliente")]
    public int IdCliente { get; set; }

    public string Dni { get; set; } = "";

    [Required(ErrorMessage = "El diagnóstico es obligatorio")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string Diagnostico { get; set; } = "";

    [Required(ErrorMessage = "El tipo de procedimiento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string TipoProcedimiento { get; set; } = "";

    [Required(ErrorMessage = "El médico tratante es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Doctor { get; set; } = "";

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria")]
    public DateTime? Hora { get; set; }

    public DateTime? FechaHoraFin { get; set; }

    public int IdEquipo { get; set; }   // 0 = sin equipo

    public CirugiaRequestDto ToRequest() => new()
    {
        TipoCliente = TipoCliente,
        IdCliente = IdCliente > 0 ? IdCliente : null,
        Dni = TipoCliente == "PACIENTE_PARTICULAR" ? Dni : null,
        Diagnostico = Diagnostico,
        TipoProcedimiento = TipoProcedimiento,
        DoctorNombre = Doctor,
        FechaHoraInicio = Fecha is null || Hora is null
            ? ""
            : Fecha.Value.Date.Add(Hora.Value.TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ss"),
        FechaHoraFin = FechaHoraFin?.ToString("yyyy-MM-ddTHH:mm:ss"),
        IdEquipo = IdEquipo > 0 ? IdEquipo : null
    };

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (TipoCliente == "PACIENTE_PARTICULAR" && (Dni.Length != 8 || !Dni.All(char.IsDigit)))
            yield return new ValidationResult("Ingrese un DNI de 8 dígitos", [nameof(Dni)]);
        if (Fecha is not null && Hora is not null && FechaHoraFin is not null
            && FechaHoraFin <= Fecha.Value.Date.Add(Hora.Value.TimeOfDay))
            yield return new ValidationResult("La fecha y hora de fin debe ser posterior a la de inicio",
                [nameof(FechaHoraFin)]);
    }
}
