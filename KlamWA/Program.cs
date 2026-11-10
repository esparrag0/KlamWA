using KlamWA.Components;
using KlamWA.Seguridad;
using KlamWA.Servicios;
using KlamWA.Servicios.Rest;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<KlamWA.ViewModels.VistaPreviaRol>();
builder.Services.AddSeguridad(builder.Configuration);
builder.Services.AddHttpClient<IDocumentoIngresoService, DocumentoIngresoService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["RestApi:BaseUrl"]!.TrimEnd('/') + "/"));
builder.Services.AddHttpClient<IClienteService, ClienteService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["RestApi:BaseUrl"]!.TrimEnd('/') + "/"));
builder.Services.AddHttpClient<ISolicitudCirugiaService, SolicitudCirugiaService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["RestApi:BaseUrl"]!.TrimEnd('/') + "/"));
builder.Services.AddHttpClient<IAuditoriaService, AuditoriaService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["RestApi:BaseUrl"]!.TrimEnd('/') + "/"));
builder.Services.AddHttpClient<ICirugiaConsultaService, CirugiaConsultaService>(client =>
    client.BaseAddress = new Uri(builder.Configuration["RestApi:BaseUrl"]!.TrimEnd('/') + "/"));


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
