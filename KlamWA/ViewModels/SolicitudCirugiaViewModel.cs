using System.ComponentModel.DataAnnotations;

namespace KlamWA.ViewModels;

public class SolicitudCirugiaViewModel : IValidatableObject
{
    public string TipoCliente { get; set; } = "Particular";
    public int IdCliente { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione la clínica u hospital de destino")]
    public int IdClinica { get; set; }
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
        if (IdCliente <= 0)
            yield return new ValidationResult("Busque y seleccione al cliente solicitante", [nameof(IdCliente)]);
        if (TipoCliente == "Particular" && (Dni.Length != 8 || !Dni.All(char.IsDigit)))
            yield return new ValidationResult("Ingrese un DNI de 8 dígitos", [nameof(Dni)]);
    }
}
