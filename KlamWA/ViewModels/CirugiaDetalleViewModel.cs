namespace KlamWA.ViewModels;

/// detalle de una cirugía (RF06): el resumen más la marca del equipo y los consumibles asignados.
public class CirugiaDetalleViewModel : CirugiaResumenViewModel
{
    public string EquipoMarca { get; set; } = "";

    public string EquipoCategoria { get; set; } = "";

    public IReadOnlyDictionary<string, string> EspecificacionesEquipo { get; set; } = new Dictionary<string, string>();

    public string BandejaProcedimiento { get; set; } = "";
    public bool? BandejaEsterilizada { get; set; }
    public List<ConsumibleAsignadoViewModel> Consumibles { get; set; } = new();
}

public class ConsumibleAsignadoViewModel
{
    public int IdConsumible { get; set; }
    public string Nombre { get; set; } = "";
    public string Marca { get; set; } = "";
    public string Medida { get; set; } = "";
    public int Despachada { get; set; }

    public int? Consumida { get; set; }

    public int? Diferencia => Consumida is null ? null : Despachada - Consumida;
}
