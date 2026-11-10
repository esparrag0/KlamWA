namespace KlamWA.Dtos;

// Respuesta de solicitud (claves = columnas de buscar_solicitudes en camelCase).
public class SolicitudCirugiaDto
{
    public int IdSolicitud { get; set; }
    public string? Codigo { get; set; }          // puede no venir; ver CodigoMostrado
    public string TipoCliente { get; set; } = "";
    public int IdCliente { get; set; }
    public string ClienteNombre { get; set; } = "";
    public string? Dni { get; set; }
    public string ClinicaNombre { get; set; } = "";
    public string PacienteNombre { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public string TipoProcedimiento { get; set; } = "";
    public string DoctorNombre { get; set; } = "";
    public DateTime FechaHoraSolicitada { get; set; }
    public string? Observaciones { get; set; }
    public string EstadoSolicitud { get; set; } = "";
    public string? MotivoRechazo { get; set; }

    // Texto del estándar visual del equipo: «Por revisar», «Rechazada», etc.
    public string EstadoMostrado => EstadoSolicitud switch
    {
        "POR_REVISAR" => "Por revisar",
        "DOCUMENTACION_INCOMPLETA" => "Documentación incompleta",
        "RECHAZADA" => "Rechazada",
        "APROBADA" => "Aprobada",
        _ => EstadoSolicitud
    };

    public string ClaseEstado => EstadoSolicitud switch
    {
        "APROBADA" => "text-bg-success",
        "RECHAZADA" => "text-bg-danger",
        "DOCUMENTACION_INCOMPLETA" => "text-bg-warning",
        _ => "text-bg-primary"
    };

    public string CodigoMostrado => string.IsNullOrWhiteSpace(Codigo) ? $"SOL-{IdSolicitud:D4}" : Codigo;
}
