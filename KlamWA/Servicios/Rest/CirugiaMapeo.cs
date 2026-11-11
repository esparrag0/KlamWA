using System.Globalization;
using KlamWA.DTOs;
using KlamWA.ViewModels;

namespace KlamWA.Servicios.Rest;

internal static class CirugiaMapeo
{
    public static CirugiaResumenViewModel ToViewModel(CirugiaResumenDTO dto)
    {
        var vm = new CirugiaResumenViewModel();
        Llenar(vm, dto);
        return vm;
    }

    public static List<CirugiaResumenViewModel> ToViewModels(IEnumerable<CirugiaResumenDTO> dtos) =>
        dtos.Select(ToViewModel).ToList();

    public static CirugiaDetalleViewModel ToDetalle(CirugiaDetalleDTO dto)
    {
        var vm = new CirugiaDetalleViewModel();
        Llenar(vm, dto);

        // Marca: la deja calculada el backend; si no vino, se busca en las especificaciones del equipo.
        vm.EquipoMarca = dto.EquipoMarca ?? "";
        if (dto.Equipo is not null)
        {
            vm.EquipoCategoria = dto.Equipo.Categoria ?? "";
            vm.EspecificacionesEquipo = dto.Equipo.Especificaciones ?? new Dictionary<string, string>();
            if (vm.EquipoMarca == "")
            {
                vm.EquipoMarca = dto.Equipo.Especificaciones?
                    .FirstOrDefault(e => string.Equals(e.Key, "marca", StringComparison.OrdinalIgnoreCase)).Value ?? "";
            }
        }

        if (dto.BandejaInstrumental is { } bandeja)
        {
            vm.BandejaProcedimiento = bandeja.Procedimiento ?? "";
            vm.BandejaEsterilizada = bandeja.Esterilizado;

            // Consumo real solo en cirugías culminadas; antes del cierre queda sin valor.
            var culminada = dto.Estado == "FINALIZADA";
            var consumidos = bandeja.ConsumiblesConsumidos
                .ToDictionary(l => l.Consumible.IdConsumible, l => l.Cantidad);
            foreach (var linea in bandeja.Consumibles)
            {
                vm.Consumibles.Add(new ConsumibleAsignadoViewModel
                {
                    IdConsumible = linea.Consumible.IdConsumible,
                    Nombre = linea.Consumible.NombreComercial,
                    Marca = linea.Consumible.Marca ?? "",
                    Medida = linea.Consumible.Medida ?? "",
                    Despachada = linea.Cantidad,
                    Consumida = culminada
                        ? (consumidos.TryGetValue(linea.Consumible.IdConsumible, out var c) ? c : 0)
                        : null
                });
            }
        }
        return vm;
    }

    public static ClienteOpcionViewModel ToOpcion(ClienteDTO dto) =>
        new() { Id = dto.IdCliente, Tipo = dto.TipoCliente, Nombre = dto.Nombre };

    public static EquipoOpcionViewModel ToOpcion(EquipoDTO dto) => new()
    {
        Id = dto.IdEquipo,
        Nombre = dto.Nombre,
        Categoria = dto.Categoria ?? "",
        Marca = dto.Especificaciones?
            .FirstOrDefault(e => string.Equals(e.Key, "marca", StringComparison.OrdinalIgnoreCase)).Value ?? "",
        Disponible = dto.Disponible
    };

    // ------------------------------------------------------------------

    private static void Llenar(CirugiaResumenViewModel vm, CirugiaResumenDTO dto)
    {
        vm.Id = dto.IdCirugia;
        vm.SolicitudId = dto.IdSolicitudOrigen;
        vm.Inicio = Fecha(dto.FechaHoraInicio) ?? default;
        vm.Fin = Fecha(dto.FechaHoraFin);
        vm.Procedimiento = dto.TipoProcedimiento ?? "";
        vm.Doctor = dto.DoctorNombre ?? "";
        vm.Paciente = dto.PacienteNombre ?? "";
        vm.Diagnostico = dto.Diagnostico ?? "";
        vm.Clinica = dto.ClinicaNombre ?? "";
        vm.Cliente = dto.ClienteNombre ?? "";
        vm.TipoCliente = dto.TipoCliente ?? "";
        vm.IdCliente = dto.IdCliente;
        vm.IdEquipo = dto.IdEquipo;
        vm.Equipo = dto.EquipoNombre ?? "";
        vm.IdBandeja = dto.IdBandeja;
        vm.Bandeja = dto.BandejaTipo ?? "";
        vm.EstadoCodigo = dto.Estado ?? "";
        vm.Estado = dto.EstadoEtiqueta ?? dto.Estado ?? "";
        vm.MotivoCancelacion = dto.MotivoCancelacion ?? "";
        vm.ConsumoCompleto = dto.ConsumoCompleto;
        vm.PrecioCotizacion = dto.PrecioPactado;
        vm.EstadoCotizacion = dto.EstadoCotizacion ?? "";
    }

    private static DateTime? Fecha(string? texto) =>
        DateTime.TryParse(texto, CultureInfo.InvariantCulture, DateTimeStyles.None, out var f) ? f : null;
}
