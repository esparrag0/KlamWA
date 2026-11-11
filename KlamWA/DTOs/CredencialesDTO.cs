namespace KlamWA.DTOs;

public sealed class CredencialesDTO
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// Respuesta de POST /login (sin token, como el profesor)
public sealed class CuentaUsuarioDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    // Solo para las cuentas de cliente (clínica u hospital, o paciente particular)
    public int? IdCliente { get; set; }
    public string? TipoCliente { get; set; }
}
