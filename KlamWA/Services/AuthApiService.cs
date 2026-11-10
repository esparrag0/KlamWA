using System.Net.Http.Json;
using KlamWA.Dtos;

namespace KlamWA.Services;

// RF01. Pantalla de acceso -> POST login -> UsuarioPlataformaBL.autenticar (administrador, vendedor,
// técnico, clínica y paciente). Credenciales incorrectas = 401 con {"error": "..."}.
public class AuthApiService : KlamApiBase
{
    public AuthApiService(HttpClient http) : base(http) { }

    public async Task<ResultadoApi<UsuarioSesionDto>> LoginAsync(string userName, string password)
    {
        try
        {
            var r = await Http.PostAsJsonAsync("login", new LoginRequestDto { UserName = userName, Password = password });
            return await LeerAsync<UsuarioSesionDto>(r);
        }
        catch (HttpRequestException) { return SinConexion<UsuarioSesionDto>(); }
    }

    // Invalida el token en el servidor. Si el servidor no responde, la sesión local se cierra igual.
    public async Task LogoutAsync(string token)
    {
        try
        {
            using var req = new HttpRequestMessage(HttpMethod.Delete, "login");
            req.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            await Http.SendAsync(req);
        }
        catch (HttpRequestException) { /* sin conexión: nada que invalidar */ }
    }
}
