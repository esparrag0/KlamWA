using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// RF05: el cliente de la sesión registra y consulta sus solicitudes
public class SolicitudClienteService : ISolicitudClienteService
{
    private readonly ISesionService _sesion;
    private string BaseUrl { get; set; }

    public SolicitudClienteService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/cliente/solicitudes";
    }

    public async Task<SolicitudCirugiaDTO> RegistrarAsync(RegistrarSolicitudDTO solicitud)
    {
        (string tipoCliente, int idCliente) = await ClienteDeSesion();
        solicitud.TipoCliente = tipoCliente;
        solicitud.IdCliente = idCliente;
        return await HttpUtils.DoPost<RegistrarSolicitudDTO, SolicitudCirugiaDTO>(BaseUrl, solicitud);
    }

    public async Task<List<SolicitudCirugiaDTO>> ListarPropiasAsync()
    {
        (string tipoCliente, int idCliente) = await ClienteDeSesion();
        string url = $"{BaseUrl}?tipoCliente={Uri.EscapeDataString(tipoCliente)}&idCliente={idCliente}";
        return await HttpUtils.DoGet<List<SolicitudCirugiaDTO>>(url) ?? [];
    }

    private async Task<(string, int)> ClienteDeSesion()
    {
        string? tipoCliente = await _sesion.ObtenerTipoClienteAsync();
        int? idCliente = await _sesion.ObtenerIdClienteAsync();
        if (tipoCliente == null || idCliente == null)
        {
            throw new ServicioException("La cuenta no está asociada a un cliente.");
        }
        return (tipoCliente, idCliente.Value);
    }
}
