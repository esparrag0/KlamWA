using KlamWA.Components;
using KlamWA.Servicios;
using KlamWA.Servicios.Rest;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<ICirugiaService, CirugiaService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IEquipoService, EquipoService>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<KlamWA.ViewModels.VistaPreviaRol>();

builder.Services.AddScoped<IBandejaService, BandejaService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();
builder.Services.AddScoped<IDespachoService, DespachoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
