using Microsoft.AspNetCore.Http;

namespace KlamWA.Servicios;

public interface ISesionService
{
    Task<string> IniciarSesionAsync(HttpContext context, string userName, string password);
    Task CerrarSesionAsync(HttpContext context);

    // Datos del usuario de la sesión (claims de la cookie), para enviarlos al REST
    Task<int> ObtenerIdUsuarioAsync();
    Task<string> ObtenerUserNameAsync();
    Task<string> ObtenerRolAsync();
    Task<int?> ObtenerIdClienteAsync();
    Task<string?> ObtenerTipoClienteAsync();
}
