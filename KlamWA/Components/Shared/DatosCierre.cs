namespace KlamWA.Components.Shared;

// Estados fijos para recorrer las pantallas del laboratorio sin guardar datos.
public sealed class CierreCirugiaMock
{
    public int IdCirugia { get; init; }
    public string EstadoOc { get; init; } = DatosCierre.OcNoAplica;
    public string ArchivoOc { get; init; } = "";
    public string MotivoRechazo { get; init; } = "";
    public string EstadoConsumo { get; init; } = DatosCierre.ConsumoNoDisponible;

    public bool PuedeAdjuntarOc => EstadoOc is DatosCierre.OcPorAdjuntar or DatosCierre.OcRechazado;
    public bool PuedeRevisarOc => EstadoOc == DatosCierre.OcPendiente;
    public bool PuedeRegistrarConsumo => EstadoOc == DatosCierre.OcValidado && EstadoConsumo != DatosCierre.ConsumoCompleto;
    public bool PuedeVerComparativo => EstadoConsumo == DatosCierre.ConsumoCompleto;
}

public static class DatosCierre
{
    public const string OcNoAplica = "No aplica";
    public const string OcPorAdjuntar = "Por adjuntar";
    public const string OcPendiente = "Pendiente";
    public const string OcValidado = "Validado";
    public const string OcRechazado = "Rechazado";
    public const string ConsumoNoDisponible = "No disponible";
    public const string ConsumoPorRegistrar = "Por registrar";
    public const string ConsumoCompleto = "Completo";

    private static readonly IReadOnlyDictionary<int, CierreCirugiaMock> PorCirugia =
        new Dictionary<int, CierreCirugiaMock>
        {
            [1] = new() { IdCirugia = 1, EstadoOc = OcValidado, ArchivoOc = "oc-cir-001.pdf", EstadoConsumo = ConsumoCompleto },
            [6] = new() { IdCirugia = 6, EstadoOc = OcPorAdjuntar },
            [7] = new() { IdCirugia = 7, EstadoOc = OcPendiente, ArchivoOc = "oc-cir-007.pdf" },
            [8] = new() { IdCirugia = 8, EstadoOc = OcValidado, ArchivoOc = "oc-cir-008.pdf", EstadoConsumo = ConsumoPorRegistrar }
        };

    public static bool EsClinica(string cliente) =>
        DatosClientes.Lista.Any(c => c.Nombre == cliente && c.Tipo == DatosClientes.Clinica);

    public static CierreCirugiaMock De(int idCirugia) =>
        PorCirugia.TryGetValue(idCirugia, out var cierre)
            ? cierre
            : new CierreCirugiaMock { IdCirugia = idCirugia };
}
