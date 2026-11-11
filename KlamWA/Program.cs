using KlamWA.Components;
using KlamWA.Seguridad;
using KlamWA.Servicios;
using KlamWA.Servicios.Rest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<KlamWA.ViewModels.VistaPreviaRol>();
builder.Services.AddSeguridad(builder.Configuration);
builder.Services.AddScoped<ICirugiaService, CirugiaService>();
builder.Services.AddScoped<IEquipoService, EquipoService>();
builder.Services.AddScoped<IBandejaService, BandejaService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IDespachoService, DespachoService>();
builder.Services.AddScoped<IDocumentoIngresoService, DocumentoIngresoService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<ISolicitudCirugiaService, SolicitudCirugiaService>();
builder.Services.AddScoped<ISolicitudClienteService, SolicitudClienteService>();
builder.Services.AddScoped<IGestionClienteService, GestionClienteService>();
builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<IRegistroCirugiaService, RegistroCirugiaService>();
builder.Services.AddScoped<IAuditoriaService, AuditoriaService>();
builder.Services.AddScoped<ICirugiaConsultaService, CirugiaConsultaService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
app.MapSesionEndpoints();

app.Run();
