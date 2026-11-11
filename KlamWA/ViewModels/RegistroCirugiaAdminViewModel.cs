using System.ComponentModel.DataAnnotations;
using KlamWA.DTOs;

namespace KlamWA.ViewModels;

// RF09: el administrador registra una cirugía (diagnóstico, fecha, hora, procedimiento, médico y clínica destino).
// Anotaciones = ayuda de UX; la validación real la hace CirugiaBL (Java) y sus mensajes se muestran tal cual.
public class RegistroCirugiaAdminViewModel : IValidatableObject
{
    // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    [Required(ErrorMessage = "Seleccione el tipo de cliente")]
    public string TipoCliente { get; set; } = "CLINICA_HOSPITAL";

    // Con CLINICA_HOSPITAL este cliente es también la clínica destino.
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione el cliente")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "El diagnóstico es obligatorio")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string Diagnostico { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de procedimiento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string TipoProcedimiento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El médico tratante es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Doctor { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria")]
    public DateTime? Hora { get; set; }

    public DateTime? FechaHoraFin { get; set; }

    public CirugiaRegistroDTO ToDto() => new()
    {
        TipoCliente = TipoCliente,
        IdCliente = IdCliente,
        Diagnostico = Diagnostico,
        TipoProcedimiento = TipoProcedimiento,
        DoctorNombre = Doctor,
        FechaHoraInicio = Fecha is null || Hora is null
            ? string.Empty
            : Fecha.Value.Date.Add(Hora.Value.TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ss"),
        FechaHoraFin = FechaHoraFin?.ToString("yyyy-MM-ddTHH:mm:ss")
    };

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (Fecha is not null && Hora is not null && FechaHoraFin is not null
            && FechaHoraFin <= Fecha.Value.Date.Add(Hora.Value.TimeOfDay))
            yield return new ValidationResult("La fecha y hora de fin debe ser posterior a la de inicio",
                new[] { nameof(FechaHoraFin) });
    }
}
