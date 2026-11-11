namespace KlamWA.ViewModels;

// Texto y color de los estados de solicitudes y documentos, según el Design System del equipo.
public static class EstadoSolicitudTexto
{
    public static string Texto(string? estado) => estado switch
    {
        "POR_REVISAR" => "Por revisar",
        "DOCUMENTACION_INCOMPLETA" => "Documentación incompleta",
        "RECHAZADA" => "Rechazada",
        "APROBADA" => "Aprobada",
        _ => estado ?? string.Empty
    };

    // Acepta el código (APROBADA) o el texto (Aprobada). Clases definidas en klam-design-system.css.
    public static string Clase(string? estado) => (estado ?? "").ToUpperInvariant().Replace(' ', '_') switch
    {
        "APROBADA" or "VALIDADO" or "COMPLETO" => "estado-validado",
        "RECHAZADA" or "RECHAZADO" or "DOCUMENTACION_INCOMPLETA" or "DOCUMENTACIÓN_INCOMPLETA" => "estado-rechazado",
        var e when e.StartsWith("RECHAZADA") => "estado-rechazado",
        "POR_REVISAR" or "PENDIENTE" or "POR_ADJUNTAR" or "POR_REGISTRAR" => "estado-pendiente",
        _ => "estado-neutro"
    };
}
