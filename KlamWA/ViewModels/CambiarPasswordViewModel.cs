using System.ComponentModel.DataAnnotations;
using KlamWA.Services;

namespace KlamWA.ViewModels;

// Cambio de contraseña. Las reglas reflejan la BL (Contrasenas, Java); el servidor valida de verdad.
public class CambiarPasswordViewModel : IValidatableObject
{
    [Required(ErrorMessage = "Ingrese su contraseña actual")]
    public string PasswordActual { get; set; } = "";

    [Required(ErrorMessage = "Ingrese la nueva contraseña")]
    public string PasswordNueva { get; set; } = "";

    [Required(ErrorMessage = "Confirme la nueva contraseña")]
    public string Confirmar { get; set; } = "";

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (!string.IsNullOrEmpty(PasswordNueva) && !ReglasCredenciales.PasswordValida(PasswordNueva))
            yield return new ValidationResult(ReglasCredenciales.TextoPassword, new[] { nameof(PasswordNueva) });
        if (!string.IsNullOrEmpty(PasswordNueva) && PasswordNueva == PasswordActual)
            yield return new ValidationResult("La nueva contraseña debe ser distinta de la actual", new[] { nameof(PasswordNueva) });
        if (PasswordNueva != Confirmar)
            yield return new ValidationResult("Las contraseñas no coinciden", new[] { nameof(Confirmar) });
    }
}
