namespace KlamWA.ViewModels;

// Reglas de usuario y contraseña. Deben coincidir con UsuarioPlataformaBLImpl y Contrasenas (Java, BL), que son las que
// validan de verdad; estos textos y validaciones solo evitan un viaje inútil al servidor.
public static class ReglasCredenciales
{
    public const int MinUsuario = 4;
    public const int MaxUsuario = 50;
    public const int MinPassword = 8;
    public const int MaxPassword = 64;

    public const string TextoUsuario =
        "Entre 4 y 50 caracteres: letras, números, punto, guion y guion bajo.";

    public const string TextoPassword =
        "Entre 8 y 64 caracteres, con al menos una letra y un número, sin espacios.";

    public static bool PasswordValida(string? password) =>
        password is { Length: >= MinPassword and <= MaxPassword }
        && password.Any(char.IsLetter)
        && password.Any(char.IsDigit)
        && !password.Any(char.IsWhiteSpace);

    public static bool UsuarioValido(string? usuario) =>
        usuario?.Trim() is { Length: >= MinUsuario and <= MaxUsuario } u
        && u.All(c => char.IsAsciiLetterOrDigit(c) || c is '.' or '-' or '_');
}
