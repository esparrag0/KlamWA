using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ICuentaUsuarioService
{
    // null si el usuario o la contraseña no son correctos
    Task<CuentaUsuarioDTO?> AutenticarAsync(CredencialesDTO credenciales);
}
