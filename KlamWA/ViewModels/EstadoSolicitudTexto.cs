namespace KlamWA.ViewModels;

// Texto y color de los estados de una solicitud, según el estándar visual del equipo.
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

    public static string Clase(string? estado) => estado switch
    {
        "APROBADA" => "text-bg-success",
        "RECHAZADA" => "text-bg-danger",
        "DOCUMENTACION_INCOMPLETA" => "text-bg-warning",
        _ => "text-bg-primary"
    };
}
