using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class SolicitudCirugiaService(
    HttpClient client,
    AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : ISolicitudCirugiaService
{
    public async Task<List<SolicitudCirugiaDTO>> ListarAsync() =>
        await HttpUtils.DoGetAutenticado<List<SolicitudCirugiaDTO>>(
            client, "solicitudes", await tokens.ObtenerAsync(autenticacion)) ?? [];

    public async Task<List<SolicitudCirugiaDTO>> ListarMiasAsync() =>
        await HttpUtils.DoGetAutenticado<List<SolicitudCirugiaDTO>>(
            client, "solicitudes/mias", await tokens.ObtenerAsync(autenticacion)) ?? [];

    public async Task<SolicitudCirugiaDTO> RegistrarAsync(RegistrarSolicitudDTO solicitud) =>
        await HttpUtils.DoPostAutenticado<RegistrarSolicitudDTO, SolicitudCirugiaDTO>(
            client, "solicitudes", await tokens.ObtenerAsync(autenticacion), solicitud)
            ?? throw new HttpRequestException("La respuesta de la solicitud está vacía.");

    public async Task<SolicitudCirugiaDTO> ConsultarAsync(int idSolicitud) =>
        await HttpUtils.DoGetAutenticado<SolicitudCirugiaDTO>(
            client, $"solicitudes/{idSolicitud}", await tokens.ObtenerAsync(autenticacion))
            ?? throw new HttpRequestException("La respuesta de la solicitud está vacía.");

    public async Task AprobarAsync(int idSolicitud) =>
        await HttpUtils.DoPostAutenticado(client, $"solicitudes/{idSolicitud}/aprobacion",
            await tokens.ObtenerAsync(autenticacion), new { });

    public async Task RechazarAsync(int idSolicitud, string motivo) =>
        await HttpUtils.DoPostAutenticado(client, $"solicitudes/{idSolicitud}/rechazo",
            await tokens.ObtenerAsync(autenticacion), new { Motivo = motivo });
}
