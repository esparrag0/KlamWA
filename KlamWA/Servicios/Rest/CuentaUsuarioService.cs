using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

public sealed class CuentaUsuarioService(HttpClient client) : ICuentaUsuarioService
{
    public Task<CuentaUsuarioDTO?> AutenticarAsync(CredencialesDTO credenciales) =>
        HttpUtils.DoPost<CredencialesDTO, CuentaUsuarioDTO>(client, "login", credenciales);

    public Task CerrarSesionAsync(string token) =>
        HttpUtils.DoPostAutenticado(client, "login/logout", token, new { });
}
