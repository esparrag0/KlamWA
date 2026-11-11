namespace KlamWA.DTOs;

// PUT cuenta/password (Java: CambioPasswordDTO). El usuario sale de la sesión (token), no del cuerpo.
public sealed class CambioPasswordDTO
{
    public string PasswordActual { get; set; } = string.Empty;
    public string PasswordNueva { get; set; } = string.Empty;
}
