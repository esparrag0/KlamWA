using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// Cliente REST de la bitácora de cirugías (RF16)
public class AuditoriaService : IAuditoriaService
{
    // Formato que acepta LocalDateTime.parse en el backend.
    private const string FormatoFecha = "yyyy-MM-dd'T'HH:mm:ss";
    private string BaseUrl { get; set; }

    public AuditoriaService(IConfiguration configuracion)
    {
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/auditoria/cirugias";
    }

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

        string url = BaseUrl + (parametros.Count > 0 ? "?" + string.Join("&", parametros) : "");
        return await HttpUtils.DoGet<List<AuditoriaDTO>>(url) ?? [];
    }

    public async Task<AuditoriaDetalleDTO?> ObtenerAsync(int idAuditoria)
    {
        string url = $"{BaseUrl}/{idAuditoria}";
        try
        {
            return await HttpUtils.DoGet<AuditoriaDetalleDTO>(url);
        }
        catch (ServicioException ex) when (ex.EsNoEncontrado)
        {
            return null;
        }
    }
}
