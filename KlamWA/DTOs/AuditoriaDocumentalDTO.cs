namespace KlamWA.DTOs;

public sealed class AuditoriaDocumentalDTO
{
    public int IdEvento { get; set; }
    public int? IdDocumento { get; set; }
    public int? IdSolicitud { get; set; }
    public int? IdCirugia { get; set; }
    public DateTime FechaHora { get; set; }
    public string TipoDocumento { get; set; } = "";
    public string Accion { get; set; } = "";
    public string Responsable { get; set; } = "";
    public string? RolResponsable { get; set; }
    public int? IdUsuarioResponsable { get; set; }
    public string? EstadoAnterior { get; set; }
    public string? EstadoNuevo { get; set; }
    public string Descripcion { get; set; } = "";
    public string? Referencia { get; set; }
    public string? Cliente { get; set; }
    public string? Paciente { get; set; }
}
