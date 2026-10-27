using System.ComponentModel.DataAnnotations;
using KlamWA.Components.Shared;

namespace KlamWA.ViewModels;

// Validación de frontend (misma idea que ProductoViewModel de la sesión 17).
// Refleja las reglas de CirugiaBLImpl.validarDatos (Java). La BL sigue siendo
// la defensa real: estas anotaciones son solo ayuda de UX.
public class CirugiaViewModel : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El tipo de procedimiento es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string TipoProcedimiento { get; set; } = "";

    [Required(ErrorMessage = "El nombre del doctor es obligatorio")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
    public string Doctor { get; set; } = "";

    [Required(ErrorMessage = "La fecha y hora de inicio es obligatoria")]
    public DateTime? FechaHoraInicio { get; set; }

    public DateTime? FechaHoraFin { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un cliente")]
    public int IdCliente { get; set; }

    public int IdEquipo { get; set; }    // 0 = ninguno
    public int IdBandeja { get; set; }   // 0 = ninguna

    public string Estado { get; set; } = "Programada";

    [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
    public string MotivoCancelacion { get; set; } = "";

    [Range(0, 1_000_000, ErrorMessage = "El precio no puede ser negativo")]
    public double PrecioPactado { get; set; }

    public CirugiaViewModel() { }

    public CirugiaViewModel(CirugiaMock c)
    {
        Id = c.Id;
        TipoProcedimiento = c.Procedimiento;
        Doctor = c.Doctor;
        FechaHoraInicio = c.Inicio;
        FechaHoraFin = c.Fin;
        IdCliente = DatosAgenda.Clientes.FirstOrDefault(x => x.Value == c.Cliente).Key;
        IdEquipo = DatosAgenda.Equipos.FirstOrDefault(x => x.Value == c.Equipo).Key;
        IdBandeja = DatosAgenda.Bandejas.FirstOrDefault(x => x.Value == c.Bandeja).Key;
        Estado = c.Estado;
        MotivoCancelacion = c.MotivoCancelacion;
        PrecioPactado = c.PrecioCotizacion;
    }

    // Reglas que cruzan campos (equivalentes a las de CirugiaBLImpl)
    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (FechaHoraInicio != null && FechaHoraFin != null && FechaHoraInicio >= FechaHoraFin)
            yield return new ValidationResult(
                "La fecha y hora de inicio debe ser anterior a la de fin",
                new[] { nameof(FechaHoraFin) });

        if (Estado == "Culminada" && FechaHoraFin == null)
            yield return new ValidationResult(
                "Una cirugía culminada debe tener fecha y hora de fin",
                new[] { nameof(FechaHoraFin) });

        if (Estado == "Cancelada" && string.IsNullOrWhiteSpace(MotivoCancelacion))
            yield return new ValidationResult(
                "Una cirugía cancelada debe tener un motivo de cancelación",
                new[] { nameof(MotivoCancelacion) });
    }
}
