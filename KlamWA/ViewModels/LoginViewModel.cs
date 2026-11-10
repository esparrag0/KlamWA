using System.ComponentModel.DataAnnotations;

namespace KlamWA.ViewModels;

// Formulario de acceso. Solo valida que no estén vacíos: la autenticación real la hace el REST/BL.
public class LoginViewModel
{
    [Required(ErrorMessage = "El usuario es obligatorio")]
    [StringLength(50, ErrorMessage = "Máximo 50 caracteres")]
    public string UserName { get; set; } = "";

    [Required(ErrorMessage = "La contraseña es obligatoria")]
    public string Password { get; set; } = "";
}
