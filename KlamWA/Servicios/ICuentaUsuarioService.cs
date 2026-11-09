using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ICuentaUsuarioService
{
    Task<CuentaUsuarioDTO?> AutenticarAsync(CredencialesDTO credenciales);
}
