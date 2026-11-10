using System.ComponentModel.DataAnnotations;
using KlamWA.DTOs;

namespace KlamWA.ViewModels;

// RF05: solicitud que registra el propio cliente. Las anotaciones son ayuda de UX; la validación real
// (obligatorios, longitudes, cliente existente) la hace SolicitudCirugiaBL (Java) y sus mensajes se muestran tal cual.
public class SolicitudClienteViewModel : IValidatableObject
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione la clínica u hospital donde se realizará")]
    public int IdClinica { get; set; }

    // La BL lo exige para clínicas; para un paciente particular, si falta, usa su propio nombre.
    public bool PacienteObligatorio { get; set; } = true;

    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string PacienteNombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El diagnóstico es obligatorio")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string Diagnostico { get; set; } = string.Empty;

    [Required(ErrorMessage = "El procedimiento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Procedimiento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre del médico es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Medico { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria")]
    public DateTime? Hora { get; set; }

    [StringLength(500, ErrorMessage = "Máximo 500 caracteres")]
    public string Observaciones { get; set; } = string.Empty;

    // Une Fecha + Hora en un ISO-8601 y arma el DTO del REST. El solicitante lo toma el servidor de la sesión.
    public RegistrarSolicitudDTO ToDto() => new()
    {
        IdClinicaDestino = IdClinica,
        PacienteNombre = PacienteNombre.Trim(),
        Diagnostico = Diagnostico,
        TipoProcedimiento = Procedimiento,
        DoctorNombre = Medico,
        FechaHoraSolicitada = Fecha is null || Hora is null
            ? string.Empty
            : Fecha.Value.Date.Add(Hora.Value.TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ss"),
        Observaciones = string.IsNullOrWhiteSpace(Observaciones) ? null : Observaciones.Trim()
    };

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (PacienteObligatorio && string.IsNullOrWhiteSpace(PacienteNombre))
            yield return new ValidationResult("El nombre del paciente es obligatorio", new[] { nameof(PacienteNombre) });
    }
}
