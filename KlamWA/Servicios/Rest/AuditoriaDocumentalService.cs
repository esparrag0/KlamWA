using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class AuditoriaDocumentalService(
    HttpClient client, AuthenticationStateProvider autenticacion, SesionTokenStore tokens)
    : IAuditoriaDocumentalService
{
    public async Task<List<AuditoriaDocumentalDTO>> BuscarAsync(string? cliente, string? paciente,
        DateTime? desde, DateTime? hasta, string? tipoDocumento, string? accion,
        string? responsable, int? idSolicitud, int? idCirugia, int pagina, int tamano)
    {
        var parametros = new List<string>();
        Agregar("cliente", cliente);
        Agregar("paciente", paciente);
        Agregar("desde", desde?.ToString("yyyy-MM-dd"));
        Agregar("hasta", hasta?.ToString("yyyy-MM-dd"));
        Agregar("tipoDocumento", tipoDocumento);
        Agregar("accion", accion);
        Agregar("responsable", responsable);
        Agregar("idSolicitud", idSolicitud?.ToString());
        Agregar("idCirugia", idCirugia?.ToString());
        Agregar("pagina", pagina.ToString());
        Agregar("tamano", tamano.ToString());
        var ruta = "auditoria/documental?" + string.Join("&", parametros);
        return await HttpUtils.DoGetAutenticado<List<AuditoriaDocumentalDTO>>(
            client, ruta, await tokens.ObtenerAsync(autenticacion)) ?? [];

        void Agregar(string clave, string? valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
                parametros.Add($"{clave}={Uri.EscapeDataString(valor.Trim())}");
        }
    }

    public async Task<AuditoriaDocumentalDTO?> ObtenerAsync(int idEvento) =>
        await HttpUtils.DoGetAutenticado<AuditoriaDocumentalDTO>(
            client, $"auditoria/documental/{idEvento}", await tokens.ObtenerAsync(autenticacion));
}
