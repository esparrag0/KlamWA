using System.Globalization;
using KlamWA.DTOs;
using KlamWA.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class CirugiaService : ICirugiaService
{
    private readonly string _baseUrl;

    public CirugiaService(IConfiguration configuracion)
    {
        _baseUrl = ServicioBase.Url(configuracion, "cirugias");
    }

    public async Task<List<CirugiaResumenViewModel>> Buscar(FiltroCirugia? filtro = null)
    {
        var parametros = new Dictionary<string, string?>();
        if (filtro is not null)
        {
            Agregar(parametros, "desde", Fecha(filtro.Desde));
            Agregar(parametros, "hasta", Fecha(filtro.Hasta));
            Agregar(parametros, "tipoCliente", filtro.TipoCliente);
            Agregar(parametros, "idCliente", filtro.IdCliente?.ToString(CultureInfo.InvariantCulture));
            Agregar(parametros, "idEquipo", filtro.IdEquipo?.ToString(CultureInfo.InvariantCulture));
            Agregar(parametros, "estado", filtro.Estado);
        }

        var url = QueryHelpers.AddQueryString(_baseUrl, parametros);
        var dtos = await HttpUtils.DoGet<List<CirugiaResumenDTO>>(url);
        return CirugiaMapeo.ToViewModels(dtos);
    }

    public async Task<List<CirugiaResumenViewModel>> ObtenerCalendario(int anio, int mes)
    {
        var url = QueryHelpers.AddQueryString($"{_baseUrl}/calendario", new Dictionary<string, string?>
        {
            ["anio"] = anio.ToString(CultureInfo.InvariantCulture),
            ["mes"] = mes.ToString(CultureInfo.InvariantCulture)
        });
        var dtos = await HttpUtils.DoGet<List<CirugiaResumenDTO>>(url);
        return CirugiaMapeo.ToViewModels(dtos);
    }

    public Task<List<CirugiaResumenViewModel>> HistorialDeCliente(string tipoCliente, int idCliente) =>
        Buscar(new FiltroCirugia(TipoCliente: tipoCliente, IdCliente: idCliente));

    public async Task<CirugiaDetalleViewModel?> FindById(int id)
    {
        try
        {
            var dto = await HttpUtils.DoGet<CirugiaDetalleDTO>($"{_baseUrl}/{id}");
            return CirugiaMapeo.ToDetalle(dto);
        }
        catch (ServicioException ex) when (ex.EsNoEncontrado)
        {
            return null;
        }
    }

    // ------------------------------------------------------------------

    private static void Agregar(Dictionary<string, string?> parametros, string nombre, string? valor)
    {
        if (!string.IsNullOrWhiteSpace(valor))
        {
            parametros[nombre] = valor.Trim();
        }
    }

    /// Solo fecha (yyyy-MM-dd) si no tiene hora, para que "hasta" incluya el día completo en el backend.</summary>
    private static string? Fecha(DateTime? fecha) =>
        fecha is null ? null
        : fecha.Value.TimeOfDay == TimeSpan.Zero
            ? fecha.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
            : fecha.Value.ToString("yyyy-MM-dd'T'HH:mm:ss", CultureInfo.InvariantCulture);
}
