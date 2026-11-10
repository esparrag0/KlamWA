namespace KlamWA.DTOs;

public sealed class DocumentoIngresoDTO
{
    public int IdDocumento { get; set; }
    public int? IdSolicitud { get; set; }
    public string TipoDocumento { get; set; } = string.Empty;
    public string EstadoValidacion { get; set; } = string.Empty;
    public int Version { get; set; }
    public DateTime? FechaCarga { get; set; }
    public DateTime? FechaValidacion { get; set; }
    public string? MotivoRechazo { get; set; }
    public bool HabilitarResubida { get; set; }
}

public sealed class ArchivoDocumentoDTO
{
    public byte[] Contenido { get; set; } = [];
    public string TipoContenido { get; set; } = "application/octet-stream";
    public string Nombre { get; set; } = "documento";
}

public sealed class HistorialDocumentoDTO
{
    public int IdDocumento { get; set; }
    public DateTime FechaHora { get; set; }
    public string Documento { get; set; } = string.Empty;
    public int Version { get; set; }
    public string? Responsable { get; set; }
    public string Estado { get; set; } = string.Empty;
    public string? MotivoRechazo { get; set; }
    public bool HabilitarResubida { get; set; }
    public DateTime? FechaValidacion { get; set; }
    public string? Referencia { get; set; }
}

public sealed class VerificacionDocumentosDTO
{
    public bool DocumentacionCompleta { get; set; }
}
