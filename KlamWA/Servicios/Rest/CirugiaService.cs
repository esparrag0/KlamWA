using System.Globalization;
using KlamWA.DTOs;
using KlamWA.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class CirugiaService : ICirugiaService
{
    private readonly ISesionService _sesion;
    private readonly string _baseUrl;

    public CirugiaService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        _baseUrl = ServicioBase.Url(configuracion, "cirugias");
    }

    public async Task<List<CirugiaResumenViewModel>> Buscar(FiltroCirugia? filtro = null)
    {
        var parametros = new Dictionary<string, string?>();
        // Un cliente solo consulta sus cirugías: su tipo e id salen de la sesión, no del filtro
        string? tipoCliente = await _sesion.ObtenerTipoClienteAsync();
        int? idCliente = await _sesion.ObtenerIdClienteAsync();
        if (tipoCliente != null && idCliente != null)
        {
            filtro = (filtro ?? new FiltroCirugia()) with { TipoCliente = tipoCliente, IdCliente = idCliente };
        }
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
        // El cliente ve en el calendario solo sus cirugías del mes
        if (await _sesion.ObtenerIdClienteAsync() != null)
        {
            DateTime inicio = new(anio, mes, 1);
            return await Buscar(new FiltroCirugia(Desde: inicio, Hasta: inicio.AddMonths(1).AddDays(-1)));
        }
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

    // ------------------------------------------------------------------ RF02 (rol 5)

    public async Task<CirugiaDTO?> ObtenerAsync(int id)
    {
        try
        {
            return await HttpUtils.DoGet<CirugiaDTO>($"{_baseUrl}/{id}");
        }
        catch (ServicioException ex) when (ex.EsNoEncontrado)
        {
            return null;
        }
    }

    public Task<OpcionesCirugiaDTO> OpcionesAsync() =>
        HttpUtils.DoGet<OpcionesCirugiaDTO>($"{_baseUrl}/opciones");

    public async Task<CirugiaDTO> RegistrarAsync(CirugiaFormDTO cirugia) =>
        await HttpUtils.DoPost<CirugiaFormDTO, CirugiaDTO>(await ConResponsable(_baseUrl), cirugia);

    public async Task<CirugiaDTO> ModificarAsync(int id, CirugiaFormDTO cirugia) =>
        await HttpUtils.DoPut<CirugiaFormDTO, CirugiaDTO>(await ConResponsable($"{_baseUrl}/{id}"), cirugia);

    public async Task<CirugiaDTO> CambiarEstadoAsync(int id, string estado) =>
        await HttpUtils.DoPost<AccionCirugiaDTO, CirugiaDTO>(await ConResponsable($"{_baseUrl}/{id}/estado"),
            new AccionCirugiaDTO { Estado = estado });

    public async Task<CirugiaDTO> CancelarAsync(int id, string motivo) =>
        await HttpUtils.DoPost<AccionCirugiaDTO, CirugiaDTO>(await ConResponsable($"{_baseUrl}/{id}/cancelacion"),
            new AccionCirugiaDTO { Motivo = motivo });

    public async Task EliminarAsync(int id) => await HttpUtils.DoDelete(await ConResponsable($"{_baseUrl}/{id}"));

    // El usuario de la sesión queda como responsable en la bitácora (RF16)
    private async Task<string> ConResponsable(string url) =>
        QueryHelpers.AddQueryString(url, "responsable", await _sesion.ObtenerUserNameAsync());

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
