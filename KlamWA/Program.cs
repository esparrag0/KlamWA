using KlamWA.Components;
using KlamWA.Servicios;
using KlamWA.Servicios.Rest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<KlamWA.ViewModels.VistaPreviaRol>();

// Servicios REST del backend Java (RF02 cirugías, RF13 plantillas de bandejas)
builder.Services.AddScoped<ICirugiaService, CirugiaService>();
builder.Services.AddScoped<IBandejaService, BandejaService>();

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
