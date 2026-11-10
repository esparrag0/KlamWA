using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class SolicitudClienteService(
    HttpClient client,
    AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : ISolicitudClienteService
{
    public async Task<SolicitudCirugiaDTO> RegistrarAsync(RegistrarSolicitudDTO solicitud) =>
        await HttpUtilsAutenticado.DoPost<RegistrarSolicitudDTO, SolicitudCirugiaDTO>(
            client, "cliente/solicitudes", await tokens.ObtenerAsync(autenticacion), solicitud)
            ?? throw new HttpRequestException("La respuesta de la solicitud está vacía.");

    public async Task<List<SolicitudCirugiaDTO>> ListarPropiasAsync() =>
        await HttpUtilsAutenticado.DoGet<List<SolicitudCirugiaDTO>>(
            client, "cliente/solicitudes", await tokens.ObtenerAsync(autenticacion)) ?? [];
}
