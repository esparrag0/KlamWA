using KlamWA.Servicios;
using KlamWA.Servicios.Impl;
using KlamWA.Servicios.Rest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace KlamWA.Seguridad;

public static class SeguridadExtensions
{
    public static IServiceCollection AddSeguridad(this IServiceCollection services, IConfiguration configuration)
    {
        var baseUrl = configuration["RestApi:BaseUrl"]
            ?? throw new InvalidOperationException("Falta RestApi:BaseUrl en appsettings.");

        services.AddMemoryCache();
        services.AddSingleton<SesionTokenStore>();
        services.AddHttpClient<ICuentaUsuarioService, CuentaUsuarioService>(client =>
            client.BaseAddress = new Uri(baseUrl.TrimEnd('/') + "/"));
        services.AddScoped<ISesionService, SesionServiceImpl>();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                // El token del backend vive en la memoria del servidor web: si se reinició (o venció), la cookie ya no
                // sirve. Se descarta para volver al login en lugar de fallar dentro de cada pantalla.
                options.Events.OnValidatePrincipal = async contexto =>
                {
                    var tokens = contexto.HttpContext.RequestServices.GetRequiredService<SesionTokenStore>();
                    if (contexto.Principal is null || tokens.Obtener(contexto.Principal) is null)
                    {
                        contexto.RejectPrincipal();
                        await contexto.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                };
            });
        services.AddAuthorization();
        services.AddCascadingAuthenticationState();
        return services;
    }

    public static void MapSesionEndpoints(this WebApplication app)
    {
        app.MapPost("/cuenta/login", async (
            [FromForm] string userName, [FromForm] string password,
            HttpContext context, ISesionService sesion) =>
        {
            var destino = await sesion.IniciarSesionAsync(context, userName, password);
            return Results.LocalRedirect(destino);
        });

        // Sesión vencida en el backend (p. ej. se reinició): se limpia la cookie local y se vuelve al login.
        app.MapGet("/cuenta/sesion-expirada", async (HttpContext context, ISesionService sesion) =>
        {
            try { await sesion.CerrarSesionAsync(context); }
            catch (Exception) { await context.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme); }
            return Results.LocalRedirect("/login?expirada=1");
        }).AllowAnonymous();

        app.MapPost("/cuenta/logout", async (HttpContext context, ISesionService sesion) =>
        {
            await sesion.CerrarSesionAsync(context);
            return Results.LocalRedirect("/login");
        }).RequireAuthorization();
    }
}
