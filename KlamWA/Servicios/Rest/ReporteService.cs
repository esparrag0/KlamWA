using System.Globalization;
using System.Net.Http.Headers;
using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

// Reportes de consumo contra el REST de Java (/reportes/...). Solo administradores (el backend lo valida).
public sealed class ReporteService(HttpClient client, AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : IReporteService
{
    private Task<string> TokenAsync() => tokens.ObtenerAsync(autenticacion);

    public Task<ReporteUsoInsumosDTO> UsoInsumosAsync(FiltroUsoInsumos filtro) =>
        ObtenerAsync<ReporteUsoInsumosDTO>("reportes/uso-insumos", Consulta(filtro));

    public Task<ArchivoReporteDTO> ExportarUsoInsumosAsync(FiltroUsoInsumos filtro, string formato) =>
        ExportarAsync("reportes/uso-insumos/exportar", Consulta(filtro, formato));

    public Task<ReporteComparativoDTO> ComparativoAsync(FiltroComparativo filtro) =>
        ObtenerAsync<ReporteComparativoDTO>("reportes/comparativo", Consulta(filtro));

    public Task<ArchivoReporteDTO> ExportarComparativoAsync(FiltroComparativo filtro, string formato) =>
        ExportarAsync("reportes/comparativo/exportar", Consulta(filtro, formato));

    public async Task<List<ConsumibleDTO>> ListarConsumiblesAsync() =>
        await ObtenerAsync<List<ConsumibleDTO>>("consumibles", new Dictionary<string, string?>());

    // ---------- Auxiliares ----------

    private async Task<T> ObtenerAsync<T>(string ruta, Dictionary<string, string?> consulta)
    {
        string url = QueryHelpers.AddQueryString(ruta, consulta);
        try
        {
            return await HttpUtilsAutenticado.DoGet<T>(client, url, await TokenAsync())
                ?? throw new ServicioException("La respuesta del reporte está vacía.");
        }
        catch (HttpRequestException ex)
        {
            throw new ServicioException(ex.Message, (int?)ex.StatusCode, ex);
        }
    }

    private async Task<ArchivoReporteDTO> ExportarAsync(string ruta, Dictionary<string, string?> consulta)
    {
        string url = QueryHelpers.AddQueryString(ruta, consulta);
        using var peticion = new HttpRequestMessage(HttpMethod.Get, url);
        peticion.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await TokenAsync());
        try
        {
            using var respuesta = await client.SendAsync(peticion);
            await HttpUtils.VerificarRespuestaAsync(respuesta);
            return new ArchivoReporteDTO
            {
                Nombre = respuesta.Content.Headers.ContentDisposition?.FileName ?? "reporte",
                Contenido = await respuesta.Content.ReadAsByteArrayAsync(),
                TipoContenido = respuesta.Content.Headers.ContentType?.MediaType ?? "application/octet-stream"
            };
        }
        catch (HttpRequestException ex)
        {
            throw new ServicioException(ex.Message, (int?)ex.StatusCode, ex);
        }
    }

    private static Dictionary<string, string?> Consulta(FiltroUsoInsumos f) => Agregar(new()
    {
        ["desde"] = Fecha(f.Desde),
        ["hasta"] = Fecha(f.Hasta),
        ["tipoCirugia"] = f.TipoCirugia,
        ["idConsumible"] = f.IdConsumible?.ToString(CultureInfo.InvariantCulture),
        ["medida"] = f.Medida
    });

    private static Dictionary<string, string?> Consulta(FiltroUsoInsumos f, string formato)
    {
        var consulta = Consulta(f);
        consulta["formato"] = formato;
        return consulta;
    }

    private static Dictionary<string, string?> Consulta(FiltroComparativo f) => Agregar(new()
    {
        ["desde"] = Fecha(f.Desde),
        ["hasta"] = Fecha(f.Hasta),
        ["idCirugia"] = f.IdCirugia?.ToString(CultureInfo.InvariantCulture),
        ["tipoCliente"] = f.TipoCliente,
        ["idCliente"] = f.IdCliente?.ToString(CultureInfo.InvariantCulture)
    });

    private static Dictionary<string, string?> Consulta(FiltroComparativo f, string formato)
    {
        var consulta = Consulta(f);
        consulta["formato"] = formato;
        return consulta;
    }

    // Quita los criterios vacíos: el backend trata una ausencia como "sin filtro".
    private static Dictionary<string, string?> Agregar(Dictionary<string, string?> consulta) =>
        consulta.Where(p => !string.IsNullOrWhiteSpace(p.Value))
            .ToDictionary(p => p.Key, p => p.Value);

    private static string? Fecha(DateOnly? fecha) =>
        fecha?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}