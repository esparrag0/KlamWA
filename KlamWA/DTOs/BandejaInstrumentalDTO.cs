namespace KlamWA.DTOs;

// Espejo de BandejaInstrumentalDTO (Java). Consumibles = despachados; ConsumiblesConsumidos = usados en la cirugía.
public class BandejaInstrumentalDTO
{
    public int IdBandeja { get; set; }
    public bool Activo { get; set; }
    public string Tipo { get; set; } = "";
    public string? Procedimiento { get; set; }
    public bool Esterilizado { get; set; }
    public List<LineaConsumibleDTO> Consumibles { get; set; } = new();
    public List<LineaConsumibleDTO> ConsumiblesConsumidos { get; set; } = new();
}
