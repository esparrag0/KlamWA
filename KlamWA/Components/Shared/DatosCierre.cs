namespace KlamWA.Components.Shared;

// Cierre post-cirugía (Lab 9, datos en memoria).
// Culminada → clínica sube OC → admin valida → registra consumo → comparativo.
public class CierreCirugiaMock
{
    public int IdCirugia { get; set; }
    public string EstadoOc { get; set; } = DatosCierre.OcNoAplica;
    public string ArchivoOc { get; set; } = "";
    public string MotivoRechazo { get; set; } = "";
    public string EstadoConsumo { get; set; } = DatosCierre.ConsumoNoDisponible;

    public bool PuedeAdjuntarOc =>
        EstadoOc is DatosCierre.OcPorAdjuntar or DatosCierre.OcRechazado;

    public bool PuedeRevisarOc => EstadoOc == DatosCierre.OcPendiente;

    public bool PuedeRegistrarConsumo =>
        EstadoOc == DatosCierre.OcValidado && EstadoConsumo != DatosCierre.ConsumoCompleto;

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

    public static List<CierreCirugiaMock> Lista { get; } =
    [
        new() { IdCirugia = 1, EstadoOc = OcPorAdjuntar, EstadoConsumo = ConsumoNoDisponible }
    ];

    public static bool EsClinica(string cliente) =>
        DatosClientes.Lista.Any(c => c.Nombre == cliente && c.Tipo == DatosClientes.Clinica);

    public static CierreCirugiaMock De(int idCirugia)
    {
        var cierre = Lista.FirstOrDefault(x => x.IdCirugia == idCirugia);
        if (cierre != null) return cierre;
        cierre = new CierreCirugiaMock { IdCirugia = idCirugia };
        Lista.Add(cierre);
        return cierre;
    }

    public static void AlCulminar(CirugiaMock cirugia)
    {
        if (cirugia.Estado != "Culminada" || !EsClinica(cirugia.Cliente)) return;

        var cierre = De(cirugia.Id);
        if (cierre.EstadoOc is OcNoAplica or "")
            cierre.EstadoOc = OcPorAdjuntar;
    }

    public static void RegistrarEnvioOc(int idCirugia, string archivo)
    {
        var cierre = De(idCirugia);
        cierre.ArchivoOc = string.IsNullOrWhiteSpace(archivo) ? $"oc-cir-{idCirugia:000}.pdf" : archivo;
        cierre.EstadoOc = OcPendiente;
        cierre.MotivoRechazo = "";
    }

    public static void ValidarOc(int idCirugia)
    {
        var cierre = De(idCirugia);
        cierre.EstadoOc = OcValidado;
        cierre.MotivoRechazo = "";
        if (cierre.EstadoConsumo != ConsumoCompleto)
            cierre.EstadoConsumo = ConsumoPorRegistrar;
    }

    public static void RechazarOc(int idCirugia, string motivo)
    {
        var cierre = De(idCirugia);
        cierre.EstadoOc = OcRechazado;
        cierre.MotivoRechazo = motivo;
        cierre.EstadoConsumo = ConsumoNoDisponible;
    }

    public static void CompletarConsumo(int idCirugia)
    {
        var cierre = De(idCirugia);
        cierre.EstadoConsumo = ConsumoCompleto;
    }
}
