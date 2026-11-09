using System.Security.Claims;
using KlamWA.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KlamWA.Servicios.Impl;

public sealed class SesionServiceImpl(ICuentaUsuarioService cuentaUsuarioService) : ISesionService
{
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
        catch (HttpRequestException)
        {
            return "/login?error=servicio";
        }
        catch (TaskCanceledException)
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
            _ => null
        };
        if (perfil is null)
            return "/login?error=rol";

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.UserName),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity));
        return $"/inicio/{perfil}";
    }

    public Task CerrarSesionAsync(HttpContext context) =>
        context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
}
