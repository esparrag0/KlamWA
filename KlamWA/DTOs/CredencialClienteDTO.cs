namespace KlamWA.DTOs;

// Credenciales que devuelve el backend al habilitar la cuenta de un cliente.
// Solo viajan una vez: la contraseña temporal no se puede volver a consultar.
public sealed class CredencialClienteDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string PasswordTemporal { get; set; } = string.Empty;
    public string TipoCliente { get; set; } = string.Empty;
    public int IdCliente { get; set; }
}
