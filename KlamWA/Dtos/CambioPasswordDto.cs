namespace KlamWA.Dtos;

// PUT cuenta/password (Java: CambioPasswordDTO). El usuario sale de la sesión (token).
public class CambioPasswordDto
{
    public string PasswordActual { get; set; } = "";
    public string PasswordNueva { get; set; } = "";
}
