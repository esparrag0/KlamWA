namespace KlamWA.Servicios.Rest;

internal static class ServicioBase
{
    public static string Url(IConfiguration configuracion, string recurso)
    {
        var baseUrl = configuracion["RestApi:BaseUrl"];
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "Falta la configuración RestApi:BaseUrl en appsettings.json (URL base del backend Java).");
        }
        return $"{baseUrl.TrimEnd('/')}/{recurso.TrimStart('/')}";
    }
}
