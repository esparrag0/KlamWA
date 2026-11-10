using KlamWA.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<KlamWA.ViewModels.VistaPreviaRol>();
builder.Services.AddScoped<KlamWA.Services.SesionUsuario>();

// REST Java (Lab 11). Un HttpClient tipado por servicio; la URL base sale de appsettings ("KlamApi:BaseUrl").
var urlApi = builder.Configuration["KlamApi:BaseUrl"]
    ?? throw new InvalidOperationException("Falta KlamApi:BaseUrl en appsettings");
builder.Services.AddHttpClient<KlamWA.Services.SolicitudesApiService>(c => c.BaseAddress = new Uri(urlApi));
builder.Services.AddHttpClient<KlamWA.Services.CirugiasApiService>(c => c.BaseAddress = new Uri(urlApi));
builder.Services.AddHttpClient<KlamWA.Services.ClientesApiService>(c => c.BaseAddress = new Uri(urlApi));
builder.Services.AddHttpClient<KlamWA.Services.AuthApiService>(c => c.BaseAddress = new Uri(urlApi));
builder.Services.AddHttpClient<KlamWA.Services.CuentaApiService>(c => c.BaseAddress = new Uri(urlApi));

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
