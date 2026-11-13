using System.Security.Claims;
using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace KlamWA.Servicios.Impl;

public sealed class SesionServiceImpl(
    ICuentaUsuarioService cuentaUsuarioService,
    SesionTokenStore tokens) : ISesionService
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
            "TECNICO_INSTRUMENTAL" or "TECNICO_INSTRUMENTISTA" => "instrumentista",
            "CLINICA_HOSPITAL" or "PACIENTE_PARTICULAR" => "cliente",
            _ => null
        };
        if (perfil is null)
            return "/login?error=rol";
        if (string.IsNullOrWhiteSpace(usuario.Token) || usuario.ExpiresAt <= DateTimeOffset.UtcNow)
            return "/login?error=servicio";

        var sesion = tokens.CrearClaim(usuario.Token, usuario.ExpiresAt);
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.UserName),
            new Claim("nombre_completo", string.IsNullOrWhiteSpace(usuario.NombreCompleto)
                ? usuario.UserName : usuario.NombreCompleto.Trim()),
            new Claim(ClaimTypes.Role, usuario.Rol),
            sesion
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        try
        {
            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties { ExpiresUtc = usuario.ExpiresAt });
        }
        catch
        {
            tokens.Eliminar(sesion);
            throw;
        }
        return $"/inicio/{perfil}";
    }

    public async Task CerrarSesionAsync(HttpContext context)
    {
        var token = tokens.Obtener(context.User);
        try
        {
            if (token is not null)
                await cuentaUsuarioService.CerrarSesionAsync(token);
        }
        catch (HttpRequestException)
        {
            // La sesión local se cierra aunque el backend ya haya expirado.
        }
        catch (TaskCanceledException)
        {
            // La sesión local se cierra aunque el backend no responda.
        }
        finally
        {
            tokens.Eliminar(context.User);
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
