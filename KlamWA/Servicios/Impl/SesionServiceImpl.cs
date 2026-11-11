using System.Security.Claims;
using KlamWA.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Impl;

// Como en el proyecto del profesor: el login devuelve los datos del usuario y se guardan como claims en la cookie.
// No hay token: el backend recibe en cada llamada lo que necesita (idUsuario, responsable, cliente).
public sealed class SesionServiceImpl(
    ICuentaUsuarioService cuentaUsuarioService,
    AuthenticationStateProvider autenticacion) : ISesionService
{
    public const string ClaimIdCliente = "idCliente";
    public const string ClaimTipoCliente = "tipoCliente";

    public async Task<string> IniciarSesionAsync(HttpContext context, string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            return "/login?error=credenciales";

        CuentaUsuarioDTO? usuario;
        try
        {
            usuario = await cuentaUsuarioService.AutenticarAsync(new CredencialesDTO
            {
                UserName = userName,
                Password = password
            });
        }
        catch (ServicioException)
        {
            return "/login?error=servicio";
        }

        if (usuario is null || usuario.Id <= 0 || string.IsNullOrWhiteSpace(usuario.UserName))
            return "/login?error=credenciales";

        var perfil = usuario.Rol switch
        {
            "ADMINISTRADOR" => "administrador",
            "VENDEDOR" => "vendedor",
            "TECNICO_INSTRUMENTISTA" => "instrumentista",
            "CLINICA_HOSPITAL" or "PACIENTE_PARTICULAR" => "cliente",
            _ => null
        };
        if (perfil is null)
            return "/login?error=rol";

        List<Claim> claims =
        [
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.UserName),
            new Claim(ClaimTypes.Role, usuario.Rol)
        ];
        if (usuario.IdCliente is int idCliente && !string.IsNullOrWhiteSpace(usuario.TipoCliente))
        {
            claims.Add(new Claim(ClaimIdCliente, idCliente.ToString()));
            claims.Add(new Claim(ClaimTipoCliente, usuario.TipoCliente));
        }
        ClaimsIdentity identidad = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignInAsync(new ClaimsPrincipal(identidad));
        return $"/inicio/{perfil}";
    }

    public async Task CerrarSesionAsync(HttpContext context)
    {
        await context.SignOutAsync();
    }

    public async Task<int> ObtenerIdUsuarioAsync()
    {
        string? valor = (await UsuarioAsync()).FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(valor, out int id) ? id : 0;
    }

    public async Task<string> ObtenerUserNameAsync() =>
        (await UsuarioAsync()).Identity?.Name ?? "";

    public async Task<string> ObtenerRolAsync() =>
        (await UsuarioAsync()).FindFirst(ClaimTypes.Role)?.Value ?? "";

    public async Task<int?> ObtenerIdClienteAsync()
    {
        string? valor = (await UsuarioAsync()).FindFirst(ClaimIdCliente)?.Value;
        return int.TryParse(valor, out int id) ? id : null;
    }

    public async Task<string?> ObtenerTipoClienteAsync() =>
        (await UsuarioAsync()).FindFirst(ClaimTipoCliente)?.Value;

    // En un circuito interactivo el HttpContext no es confiable: se leen los claims del estado de autenticación
    private async Task<ClaimsPrincipal> UsuarioAsync() =>
        (await autenticacion.GetAuthenticationStateAsync()).User;
}
