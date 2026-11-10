using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class CuentaService(
    HttpClient client,
    AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : ICuentaService
{
    public async Task CambiarPasswordAsync(string passwordActual, string passwordNueva) =>
        await HttpUtilsAutenticado.DoPut(
            client, "cuenta/password", await tokens.ObtenerAsync(autenticacion),
            new CambioPasswordDTO { PasswordActual = passwordActual, PasswordNueva = passwordNueva });
}
