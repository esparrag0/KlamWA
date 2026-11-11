namespace KlamWA.DTOs;

public sealed class CredencialesDTO
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class CuentaUsuarioDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTimeOffset ExpiresAt { get; set; }
}
