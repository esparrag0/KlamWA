using System.ComponentModel.DataAnnotations;

namespace KlamWA.ViewModels;

// Solicitud de cirugía enviada por el cliente. Solo validación visual (UX):
// los campos obligatorios y longitudes se confirmarán contra la BL de Java
// (Cirugia: tipoProcedimiento, doctorNombre, fechaHoraInicio) en el lab de integración.
public class SolicitudCirugiaViewModel
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
}
