using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// RF08: solicitudes que registra el vendedor de la sesión; el administrador las revisa
public class SolicitudCirugiaService : ISolicitudCirugiaService
{
    private readonly ISesionService _sesion;
    private string BaseUrl { get; set; }

    public SolicitudCirugiaService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/solicitudes";
    }

    public async Task<List<SolicitudCirugiaDTO>> ListarAsync() =>
        await HttpUtils.DoGet<List<SolicitudCirugiaDTO>>(BaseUrl) ?? [];

    public async Task<List<SolicitudCirugiaDTO>> ListarMiasAsync() =>
        await HttpUtils.DoGet<List<SolicitudCirugiaDTO>>(
            $"{BaseUrl}/mias?idVendedor={await _sesion.ObtenerIdUsuarioAsync()}") ?? [];

    public async Task<SolicitudCirugiaDTO> RegistrarAsync(RegistrarSolicitudDTO solicitud)
    {
        solicitud.IdVendedor = await _sesion.ObtenerIdUsuarioAsync();
        return await HttpUtils.DoPost<RegistrarSolicitudDTO, SolicitudCirugiaDTO>(BaseUrl, solicitud);
    }

    public async Task<SolicitudCirugiaDTO> ConsultarAsync(int idSolicitud) =>
        await HttpUtils.DoGet<SolicitudCirugiaDTO>($"{BaseUrl}/{idSolicitud}");

    public async Task AprobarAsync(int idSolicitud) =>
        await HttpUtils.DoPost($"{BaseUrl}/{idSolicitud}/aprobacion");

    public async Task RechazarAsync(int idSolicitud, string motivo) =>
        await HttpUtils.DoPost($"{BaseUrl}/{idSolicitud}/rechazo", new { Motivo = motivo });
}
