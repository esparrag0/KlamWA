using System.ComponentModel.DataAnnotations;

namespace KlamWA.ViewModels;

/// Una cirugía finalizada en la pantalla de consumos (RF15). Los textos de estado son los mismos que usan
/// las etiquetas del Design System (DocumentoEstadoBadge).
public class ConsumoCirugiaViewModel
{
    public const string Clinica = "CLINICA_HOSPITAL";

    public int IdCirugia { get; set; }
    public string Codigo => $"CIR-{IdCirugia:000}";
    public DateTime? Inicio { get; set; }
    public string Procedimiento { get; set; } = "";
    public string TipoCliente { get; set; } = "";
    public string Cliente { get; set; } = "";
    public int? IdBandeja { get; set; }
    public bool RequiereOrdenCompra { get; set; }
    public string EstadoOrdenCompra { get; set; } = "";
    public bool DocumentacionCierreCompleta { get; set; }
    public string EstadoConsumo { get; set; } = "";

    public bool EsClinica => TipoCliente == Clinica;

    public bool PuedeRegistrarConsumo => EstadoConsumo == "POR_REGISTRAR";
    public bool PuedeVerComparativo => EstadoConsumo == "COMPLETO";
    public bool PuedeRevisarOc => EsClinica && EstadoOrdenCompra == "PENDIENTE";

    public string EstadoOrdenCompraTexto => EstadoOrdenCompra switch
    {
        "NO_APLICA" => "No aplica",
        "POR_ADJUNTAR" => "Por adjuntar",
        "PENDIENTE" => "Pendiente",
        "VALIDADO" => "Validado",
        "RECHAZADO" => "Rechazado",
        _ => EstadoOrdenCompra
    };

    /// Clínicas y hospitales: estado de la orden de compra. Pacientes particulares: no llevan OC,
    /// se muestra el estado de su comprobante de pago.
    public string DocumentacionTexto => RequiereOrdenCompra
        ? EstadoOrdenCompraTexto
        : DocumentacionCierreCompleta ? "Validado" : "Pendiente";

    public string EstadoConsumoTexto => EstadoConsumo switch
    {
        "NO_DISPONIBLE" => "No disponible",
        "POR_REGISTRAR" => "Por registrar",
        "COMPLETO" => "Completo",
        _ => EstadoConsumo
    };

    /// Por qué todavía no se puede registrar el consumo (solo cuando EstadoConsumo = NO_DISPONIBLE).
    public string MotivoNoDisponible
    {
        get
        {
            if (IdBandeja is null) return "La cirugía no tiene una bandeja asignada.";
            if (DocumentacionCierreCompleta)
                return "La bandeja asignada no tiene consumibles despachados para esta cirugía: asigne una bandeja con consumibles.";
            return RequiereOrdenCompra
                ? "Primero la clínica debe adjuntar la orden de compra y administración debe validarla."
                : "Primero se debe validar el comprobante de pago del paciente.";
        }
    }
}

public class ConsumoLineaViewModel
{
    public int IdConsumible { get; set; }
    public string Nombre { get; set; } = "";
    public string Medida { get; set; } = "";
    public int CantidadDespachada { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La cantidad consumida no puede ser negativa")]
    public int CantidadConsumida { get; set; }

    public int Diferencia => CantidadDespachada - CantidadConsumida;

    public bool EsValida => CantidadConsumida >= 0 && CantidadConsumida <= CantidadDespachada;
}

public class ConsumoDetalleViewModel : ConsumoCirugiaViewModel
{
    public string Doctor { get; set; } = "";
    public string Paciente { get; set; } = "";
    public string ClinicaNombre { get; set; } = "";
    public string? ArchivoOrdenCompra { get; set; }
    public string? MotivoRechazoOrdenCompra { get; set; }
    public List<ConsumoLineaViewModel> Lineas { get; set; } = [];

    public int TotalDespachado => Lineas.Sum(l => l.CantidadDespachada);
    public int TotalConsumido => Lineas.Sum(l => l.CantidadConsumida);
    public int Diferencia => TotalDespachado - TotalConsumido;

    /// Mensajes de las líneas que no se pueden guardar (cantidad negativa o mayor que lo despachado).
    public List<string> Errores()
    {
        var errores = new List<string>();
        foreach (var l in Lineas.Where(l => !l.EsValida))
        {
            errores.Add($"{l.Nombre} ({l.Medida}): la cantidad consumida debe estar entre 0 y {l.CantidadDespachada}.");
        }
        return errores;
    }
}
