namespace KlamWA.DTOs;

public sealed class CotizacionDTO
{
    public int IdCotizacion { get; set; }
    public int IdSolicitud { get; set; }
    public int Version { get; set; }
    public bool Vigente { get; set; }
    public string Estado { get; set; } = "";
    public double PrecioPactado { get; set; }
    public DateTime FechaEmision { get; set; }
    public DateTime? FechaDecision { get; set; }
    public int? IdCuentaDecision { get; set; }
    public string? MotivoRechazo { get; set; }
    public bool ArchivoDisponible { get; set; }
}

public sealed class CotizacionCreadaDTO
{
    public int IdCotizacion { get; set; }
    public int Version { get; set; }
    public string Estado { get; set; } = "";
}

public sealed class ElegibilidadCirugiaDTO
{
    public bool PuedeRegistrarCirugia { get; set; }
    public bool RequiereCotizacion { get; set; }
    public string? EstadoDocumental { get; set; }
    public string? EstadoCotizacion { get; set; }
    public string? MotivoBloqueo { get; set; }
}