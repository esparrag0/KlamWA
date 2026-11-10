using KlamWA.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<KlamWA.ViewModels.VistaPreviaRol>();

// Servicios REST del backend Java (RF02 cirugías, RF13 plantillas de bandejas)
// TODO token: cuando el login con token llegue a master, enviar Authorization: Bearer en estos clientes
string apiRest = builder.Configuration["RestApi:BaseUrl"]
    ?? throw new InvalidOperationException("Falta RestApi:BaseUrl en appsettings.json");
builder.Services.AddHttpClient<KlamWA.Servicios.ICirugiaService, KlamWA.Servicios.Rest.CirugiaRestService>(
    cliente => cliente.BaseAddress = new Uri(apiRest.TrimEnd('/') + "/"));
builder.Services.AddHttpClient<KlamWA.Servicios.IBandejaService, KlamWA.Servicios.Rest.BandejaRestService>(
    cliente => cliente.BaseAddress = new Uri(apiRest.TrimEnd('/') + "/"));

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
