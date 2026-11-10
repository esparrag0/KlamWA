using System.ComponentModel.DataAnnotations;
using KlamWA.Dtos;

namespace KlamWA.ViewModels;

// Solicitud de cirugía enviada por el cliente (RF05). Las anotaciones son solo ayuda de UX:
// la validación real (obligatorios, longitudes, DNI, cliente existente) la hace SolicitudCirugiaBL en Java
// y sus mensajes se muestran tal cual. ToRequest() arma el DTO que viaja al REST.
public class SolicitudCirugiaViewModel : IValidatableObject
{
    // Clínica u hospital donde se realizará la cirugía (RF05). El solicitante es el cliente
    // que inició sesión, por eso no se pide aquí.
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione la clínica u hospital")]
    public int IdClinica { get; set; }

    [Required(ErrorMessage = "El diagnóstico es obligatorio")]
    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string Diagnostico { get; set; } = "";

    [Required(ErrorMessage = "El procedimiento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Procedimiento { get; set; } = "";

    [Required(ErrorMessage = "El nombre del médico es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Medico { get; set; } = "";

    [Required(ErrorMessage = "La fecha es obligatoria")]
    public DateTime? Fecha { get; set; }

    [Required(ErrorMessage = "La hora es obligatoria")]
    public DateTime? Hora { get; set; }

    [StringLength(300, ErrorMessage = "Máximo 300 caracteres")]
    public string Observaciones { get; set; } = "";

    // Cliente solicitante. TEMPORAL: sale de la sesión del cliente cuando exista login de clientes (RF01).
    [Range(1, int.MaxValue, ErrorMessage = "Indique el id del cliente solicitante")]
    public int IdSolicitante { get; set; }
    [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
    public string PacienteNombre { get; set; } = "";
    public string TipoCliente { get; set; } = "Clínica/Hospital";
    public string Dni { get; set; } = "";
    public bool ComprobanteSeleccionado { get; set; }

    // Código de tipo de cliente que entiende el REST (CirugiaBL.TIPO_CLIENTE_*).
    public string TipoClienteApi => TipoCliente == "Particular" ? "PACIENTE_PARTICULAR" : "CLINICA_HOSPITAL";

    // Une Fecha + Hora en un solo ISO-8601 y arma el DTO del REST. No valida reglas de negocio: eso lo hace la BL.
    public SolicitudCirugiaRequestDto ToRequest() => new()
    {
        IdClinicaDestino = IdClinica > 0 ? IdClinica : null,
        PacienteNombre = string.IsNullOrWhiteSpace(PacienteNombre) ? null : PacienteNombre.Trim(),
        Dni = TipoCliente == "Particular" ? Dni : null,
        Diagnostico = Diagnostico,
        TipoProcedimiento = Procedimiento,
        DoctorNombre = Medico,
        FechaHoraSolicitada = Fecha is null || Hora is null
            ? ""
            : Fecha.Value.Date.Add(Hora.Value.TimeOfDay).ToString("yyyy-MM-ddTHH:mm:ss"),
        Observaciones = string.IsNullOrWhiteSpace(Observaciones) ? null : Observaciones.Trim()
    };

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (TipoCliente != "Particular" && string.IsNullOrWhiteSpace(PacienteNombre))
            yield return new ValidationResult("El nombre del paciente es obligatorio", [nameof(PacienteNombre)]);
        if (TipoCliente == "Particular" && (Dni.Length != 8 || !Dni.All(char.IsDigit)))
            yield return new ValidationResult("Ingrese un DNI de 8 dígitos para el paciente particular", [nameof(Dni)]);
        if (TipoCliente == "Particular" && !ComprobanteSeleccionado)
            yield return new ValidationResult("Seleccione el comprobante de pago bancario", [nameof(ComprobanteSeleccionado)]);
    }
}
