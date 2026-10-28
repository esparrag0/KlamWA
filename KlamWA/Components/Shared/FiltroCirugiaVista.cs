namespace KlamWA.Components.Shared;

// Filtros de presentación aplicados únicamente a los datos de ejemplo.
public sealed record FiltroCirugiaVista(
    DateTime? Desde = null,
    DateTime? Hasta = null,
    string Cliente = "",
    string TipoCliente = "",
    string Equipo = "",
    string Estado = "");

public static class FiltrosAgenda
{
    public static IEnumerable<CirugiaMock> Aplicar(IEnumerable<CirugiaMock> cirugias, FiltroCirugiaVista filtro) =>
        cirugias.Where(c =>
            (filtro.Desde is null || c.Inicio.Date >= filtro.Desde.Value.Date) &&
            (filtro.Hasta is null || c.Inicio.Date <= filtro.Hasta.Value.Date) &&
            (filtro.Cliente == "" || c.Cliente == filtro.Cliente) &&
            (filtro.Equipo == "" || c.Equipo == filtro.Equipo) &&
            (filtro.Estado == "" || c.Estado == filtro.Estado) &&
            (filtro.TipoCliente == "" || DatosClientes.Lista.FirstOrDefault(x => x.Nombre == c.Cliente)?.Tipo == filtro.TipoCliente));
}
