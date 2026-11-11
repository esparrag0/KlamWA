using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// PUT /cuenta/{userName}/password, como /mi-cuenta/{userName}/password del profesor
public class CuentaService : ICuentaService
{
    private readonly ISesionService _sesion;
    private string BaseUrl { get; set; }

    public CuentaService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/cuenta";
    }

    public async Task CambiarPasswordAsync(string passwordActual, string passwordNueva)
    {
        string url = $"{BaseUrl}/{Uri.EscapeDataString(await _sesion.ObtenerUserNameAsync())}/password";
        await HttpUtils.DoPut(url, new CambioPasswordDTO { PasswordActual = passwordActual, PasswordNueva = passwordNueva });
    }
}
