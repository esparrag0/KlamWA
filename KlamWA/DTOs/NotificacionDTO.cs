using System.Globalization;
using System.Text.Json.Serialization;

namespace KlamWA.DTOs;

public class NotificacionDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; } = "";
    public string Mensaje { get; set; } = "";
    public string? FechaHora { get; set; }
    public bool Leida { get; set; }
    public string Categoria { get; set; } = "";
    public int? IdCirugia { get; set; }
    public int? IdSolicitud { get; set; }
    public string Destinatario { get; set; } = "";
    public string Perfil { get; set; } = "";

    [JsonIgnore]
    public DateTime? Fecha => DateTime.TryParse(FechaHora, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;

    [JsonIgnore]
    public string FechaTexto => Fecha?.ToString("dd/MM/yyyy HH:mm") ?? (FechaHora ?? "—");
}

public class NotificacionResumenDTO
{
    public int Total { get; set; }
    public int NoLeidas { get; set; }
}
