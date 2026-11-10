namespace KlamWA.ViewModels;

/// Cirugía para calendario, lista e historial (RF06, RF10, RF17). Mismos nombres que CirugiaMock
/// (Inicio, Procedimiento, Doctor, Paciente, Clinica, Cliente, Equipo, Bandeja, Estado...) para que las
/// pantallas pasen de los datos de ejemplo a los reales con pocos cambios. Es solo lectura: viene del backend.
public class CirugiaResumenViewModel
{
    public int Id { get; set; }
    public int? SolicitudId { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime? Fin { get; set; }
    public string Procedimiento { get; set; } = "";
    public string Doctor { get; set; } = "";
    public string Paciente { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public string Clinica { get; set; } = "";
    public string Cliente { get; set; } = "";
    public string TipoCliente { get; set; } = "";
    public int? IdCliente { get; set; }
    public int? IdEquipo { get; set; }
    public string Equipo { get; set; } = "";
    public int? IdBandeja { get; set; }
    public string Bandeja { get; set; } = "";



    public string Estado { get; set; } = "";

    public string EstadoCodigo { get; set; } = "";

    public string MotivoCancelacion { get; set; } = "";
    public bool ConsumoCompleto { get; set; }

    public double? PrecioCotizacion { get; set; }

    public string EstadoCotizacion { get; set; } = "";
}
