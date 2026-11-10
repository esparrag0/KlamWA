namespace KlamWA.DTOs;

// Espejo de CirugiaResumenDTO (Java): fila plana para calendario, lista e historial (RF06, RF10, RF17).
// Las fechas llegan como texto ISO-8601 (yyyy-MM-ddTHH:mm:ss). Estado es el código (PROGRAMADA, EN_PROCESO,
// FINALIZADA, CANCELADA) y EstadoEtiqueta el texto de pantalla (Programada, En curso, Culminada, Cancelada).
public class CirugiaResumenDTO
{
    public int IdCirugia { get; set; }
    public string? FechaHoraInicio { get; set; }
    public string? FechaHoraFin { get; set; }
    public string? TipoProcedimiento { get; set; }
    public string? DoctorNombre { get; set; }
    public string? Diagnostico { get; set; }
    public string? MotivoCancelacion { get; set; }
    public string? Estado { get; set; }
    public string? EstadoEtiqueta { get; set; }
    public string? TipoCliente { get; set; }
    public int? IdCliente { get; set; }
    public string? ClienteNombre { get; set; }
    public string? PacienteNombre { get; set; }
    public string? ClinicaNombre { get; set; }
    public int? IdEquipo { get; set; }
    public string? EquipoNombre { get; set; }
    public int? IdBandeja { get; set; }
    public string? BandejaTipo { get; set; }
    public int? IdSolicitudOrigen { get; set; }
    public bool ConsumoCompleto { get; set; }
    public double? PrecioPactado { get; set; }
    public string? EstadoCotizacion { get; set; }
}
