using System.Globalization;
using KlamWA.DTOs;
using KlamWA.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class ConsumoService : IConsumoService
{
    private readonly RestConSesion _rest;
    private string BaseUrl { get; set; }

    public ConsumoService(IConfiguration configuracion, RestConSesion rest)
    {
        _rest = rest;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/consumos";
    }

    public async Task<List<ConsumoCirugiaViewModel>> ListarAsync(string? busqueda = null, DateTime? desde = null, DateTime? hasta = null)
    {
        var query = new Dictionary<string, string?>();
        if (!string.IsNullOrWhiteSpace(busqueda)) query["q"] = busqueda.Trim();
        if (desde is not null) query["desde"] = desde.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        if (hasta is not null) query["hasta"] = hasta.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        string url = query.Count > 0 ? QueryHelpers.AddQueryString(BaseUrl, query) : BaseUrl;
        List<ConsumoCirugiaDTO> consumos = await _rest.DoGet<List<ConsumoCirugiaDTO>>(url);
        return consumos.Select(c => ToViewModel(c, new ConsumoCirugiaViewModel())).ToList();
    }

    public async Task<ConsumoDetalleViewModel?> ObtenerAsync(int idCirugia)
    {
        string url = $"{BaseUrl}/{idCirugia}";
        try
        {
            return ToDetalleViewModel(await _rest.DoGet<ConsumoDetalleDTO>(url));
        }
        catch (ServicioException ex) when (ex.EsNoEncontrado)
        {
            return null;
        }
    }

    public async Task<ConsumoDetalleViewModel> GuardarAsync(ConsumoDetalleViewModel consumo, bool confirmar)
    {
        string url = $"{BaseUrl}/{consumo.IdCirugia}";
        var registro = new RegistroConsumoDTO
        {
            IdCirugia = consumo.IdCirugia,
            Confirmar = confirmar,
            Lineas = consumo.Lineas.Select(l => new LineaRegistroConsumoDTO
            {
                IdConsumible = l.IdConsumible,
                CantidadConsumida = l.CantidadConsumida
            }).ToList()
        };
        ConsumoDetalleDTO guardado = await _rest.DoPut<RegistroConsumoDTO, ConsumoDetalleDTO>(url, registro);
        return ToDetalleViewModel(guardado);
    }

    private static T ToViewModel<T>(ConsumoCirugiaDTO dto, T vm) where T : ConsumoCirugiaViewModel
    {
        vm.IdCirugia = dto.IdCirugia;
        vm.Inicio = DateTime.TryParse(dto.FechaHoraInicio, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha) ? fecha : null;
        vm.Procedimiento = dto.TipoProcedimiento ?? "";
        vm.TipoCliente = dto.TipoCliente;
        vm.Cliente = dto.ClienteNombre ?? "";
        vm.IdBandeja = dto.IdBandeja;
        vm.RequiereOrdenCompra = dto.RequiereOrdenCompra;
        vm.EstadoOrdenCompra = dto.EstadoOrdenCompra;
        vm.DocumentacionCierreCompleta = dto.DocumentacionCierreCompleta;
        vm.EstadoConsumo = dto.EstadoConsumo;
        return vm;
    }

    private static ConsumoDetalleViewModel ToDetalleViewModel(ConsumoDetalleDTO dto)
    {
        var vm = ToViewModel(dto, new ConsumoDetalleViewModel());
        vm.Doctor = dto.DoctorNombre ?? "";
        vm.Paciente = dto.PacienteNombre ?? "";
        vm.ClinicaNombre = dto.ClinicaNombre ?? "";
        vm.ArchivoOrdenCompra = dto.ArchivoOrdenCompra;
        vm.MotivoRechazoOrdenCompra = dto.MotivoRechazoOrdenCompra;
        vm.Lineas = dto.Consumibles.Select(l => new ConsumoLineaViewModel
        {
            IdConsumible = l.IdConsumible,
            Nombre = l.Nombre,
            Medida = l.Medida,
            CantidadDespachada = l.CantidadDespachada,
            CantidadConsumida = l.CantidadConsumida
        }).ToList();
        return vm;
    }
}
