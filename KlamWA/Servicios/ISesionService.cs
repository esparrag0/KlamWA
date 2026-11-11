using Microsoft.AspNetCore.Http;

namespace KlamWA.Servicios;

public interface ISesionService
{
    Task<string> IniciarSesionAsync(HttpContext context, string userName, string password);
    Task CerrarSesionAsync(HttpContext context);
}
