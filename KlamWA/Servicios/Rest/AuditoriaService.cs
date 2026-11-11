using System.Net;
using System.Net.Http.Json;
using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

/// <summary>
/// Cliente REST de la bitacora de cirugias. No envia token (RF01 pendiente).
/// </summary>
public sealed class AuditoriaService(HttpClient client, AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : IAuditoriaService
{
    // Formato que acepta LocalDateTime.parse en el backend.
    private const string FormatoFecha = "yyyy-MM-dd'T'HH:mm:ss";

    public async Task<List<AuditoriaDTO>> ListarAsync(int? idCirugia, DateTime? desde, DateTime? hasta,
                                                      string? accion, string? responsable)
    {
        var parametros = new List<string>();
        if (idCirugia is not null)
            parametros.Add($"idCirugia={idCirugia.Value}");
        if (desde is not null)
            parametros.Add($"desde={Uri.EscapeDataString(desde.Value.ToString(FormatoFecha))}");
        if (hasta is not null)
            parametros.Add($"hasta={Uri.EscapeDataString(hasta.Value.ToString(FormatoFecha))}");
        if (!string.IsNullOrWhiteSpace(accion))
            parametros.Add($"accion={Uri.EscapeDataString(accion)}");
        if (!string.IsNullOrWhiteSpace(responsable))
            parametros.Add($"responsable={Uri.EscapeDataString(responsable)}");

        var ruta = "auditoria/cirugias" + (parametros.Count > 0 ? "?" + string.Join("&", parametros) : "");
        return await HttpUtilsAutenticado.DoGet<List<AuditoriaDTO>>(
            client, ruta, await tokens.ObtenerAsync(autenticacion)) ?? [];
    }

    public async Task<AuditoriaDetalleDTO?> ObtenerAsync(int idAuditoria)
    {
        try {
            return await HttpUtilsAutenticado.DoGet<AuditoriaDetalleDTO>(
                client, $"auditoria/cirugias/{idAuditoria}", await tokens.ObtenerAsync(autenticacion));
        } catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound) {
            return null;
        }
    }
}
