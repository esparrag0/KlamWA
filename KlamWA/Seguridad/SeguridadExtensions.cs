using KlamWA.Servicios;
using KlamWA.Servicios.Impl;
using KlamWA.Servicios.Rest;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace KlamWA.Seguridad;

public static class SeguridadExtensions
{
    public static IServiceCollection AddSeguridad(this IServiceCollection services, IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration["RestApi:BaseUrl"]))
            throw new InvalidOperationException("Falta RestApi:BaseUrl en appsettings.");

        services.AddScoped<ICuentaUsuarioService, CuentaUsuarioService>();
        services.AddScoped<ISesionService, SesionServiceImpl>();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/login";
                options.AccessDeniedPath = "/login";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
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

        app.MapPost("/cuenta/logout", async (HttpContext context, ISesionService sesion) =>
        {
            await sesion.CerrarSesionAsync(context);
            return Results.LocalRedirect("/login");
        }).RequireAuthorization();
    }
}
