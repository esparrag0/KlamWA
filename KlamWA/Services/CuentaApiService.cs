using System.Net.Http.Json;
using KlamWA.Dtos;

namespace KlamWA.Services;

// Cuenta del usuario con sesión: cambio de contraseña (cualquier rol). La BL exige la contraseña actual.
public class CuentaApiService : KlamApiBase
{
    public CuentaApiService(HttpClient http, SesionUsuario sesion) : base(http, sesion) { }

    public async Task<ResultadoApi<bool>> CambiarPasswordAsync(string passwordActual, string passwordNueva)
    {
        try
        {
            var dto = new CambioPasswordDto { PasswordActual = passwordActual, PasswordNueva = passwordNueva };
            return await LeerSinCuerpoAsync(await Http.PutAsJsonAsync("cuenta/password", dto));
        }
        catch (HttpRequestException) { return SinConexion<bool>(); }
    }
}
