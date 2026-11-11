using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;

namespace KlamWA.Seguridad;

// El token Java permanece en memoria del servidor; la cookie solo contiene una clave aleatoria.
public sealed class SesionTokenStore(IMemoryCache cache)
{
    private const string ClaimSesion = "klam_sesion";

    public Claim CrearClaim(string token, DateTimeOffset vence)
    {
        var clave = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        cache.Set(clave, token, new MemoryCacheEntryOptions { AbsoluteExpiration = vence });
        return new Claim(ClaimSesion, clave);
    }

    public string? Obtener(ClaimsPrincipal usuario)
    {
        var clave = usuario.FindFirst(ClaimSesion)?.Value;
        return clave is not null && cache.TryGetValue(clave, out string? token) ? token : null;
    }

    public async Task<string> ObtenerAsync(AuthenticationStateProvider autenticacion)
    {
        var usuario = (await autenticacion.GetAuthenticationStateAsync()).User;
        return Obtener(usuario) ?? throw new HttpRequestException(
            "La sesión de acceso al backend expiró. Vuelve a iniciar sesión.",
            null, HttpStatusCode.Unauthorized);
    }

    public void Eliminar(ClaimsPrincipal usuario)
    {
        var clave = usuario.FindFirst(ClaimSesion)?.Value;
        if (clave is not null) cache.Remove(clave);
    }

    public void Eliminar(Claim claim) => cache.Remove(claim.Value);
}
