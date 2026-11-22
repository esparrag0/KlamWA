using System.ComponentModel.DataAnnotations;

namespace KlamWA.ViewModels;

public class SolicitudCirugiaViewModel : IValidatableObject
{
    public string TipoCliente { get; set; } = "Particular";
    public int IdCliente { get; set; }
    public int IdClinica { get; set; }
    // Clínica que aún no está en BIOKLAM: sus datos se envían con la solicitud.
    public bool ClinicaNuevaLista { get; set; }
    public string Dni { get; set; } = string.Empty;
    [Required(ErrorMessage = "El nombre del paciente es obligatorio")]
    [StringLength(150)]
    public string PacienteNombre { get; set; } = string.Empty;
    [Required(ErrorMessage = "El diagnóstico es obligatorio")]
    [StringLength(300)]
    public string Diagnostico { get; set; } = string.Empty;
    [Required(ErrorMessage = "El procedimiento es obligatorio")]
    [StringLength(100)]
    public string Procedimiento { get; set; } = string.Empty;
    [Required(ErrorMessage = "El nombre del médico es obligatorio")]
    [StringLength(100)]
    public string Medico { get; set; } = string.Empty;
    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime? Fecha { get; set; }
    [Required(ErrorMessage = "La hora es obligatoria")]
    public DateTime? Hora { get; set; }
    [StringLength(500)]
    public string Observaciones { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (TipoCliente == "Clinica")
        {
            if (!ClinicaNuevaLista)
                yield return new ValidationResult("Busque el RUC de la clínica e ingrese sus datos si no está registrada", [nameof(ClinicaNuevaLista)]);
            yield break;
        }
        if (IdCliente <= 0)
            yield return new ValidationResult("Busque y seleccione al cliente solicitante", [nameof(IdCliente)]);
        if (IdClinica <= 0)
            yield return new ValidationResult("Seleccione la clínica u hospital de destino", [nameof(IdClinica)]);
        if (Dni.Length != 8 || !Dni.All(char.IsDigit))
            yield return new ValidationResult("Ingrese un DNI de 8 dígitos", [nameof(Dni)]);
    }
}
