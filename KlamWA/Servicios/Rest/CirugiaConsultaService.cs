using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// Cliente REST del historial de cirugías (RF10): el cliente de la sesión
public class CirugiaConsultaService : ICirugiaConsultaService
{
    private readonly ISesionService _sesion;
    private string BaseUrl { get; set; }

    public CirugiaConsultaService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/cirugias/historial";
    }

    public async Task<List<CirugiaHistorialDTO>> HistorialClienteAsync()
    {
        string? tipoCliente = await _sesion.ObtenerTipoClienteAsync();
        int? idCliente = await _sesion.ObtenerIdClienteAsync();
        if (tipoCliente == null || idCliente == null)
        {
            throw new ServicioException("La cuenta no está asociada a un cliente.");
        }
        string url = $"{BaseUrl}?tipoCliente={Uri.EscapeDataString(tipoCliente)}&idCliente={idCliente}";
        return await HttpUtils.DoGet<List<CirugiaHistorialDTO>>(url) ?? [];
    }
}
