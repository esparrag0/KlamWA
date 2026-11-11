namespace KlamWA.DTOs;

// Espejo de pe.edu.pucp.klam.rest.dto.ConsumibleDTO (Java)
public class ConsumibleDTO
{
    public int IdConsumible { get; set; }
    public bool Activo { get; set; }
    public string NombreComercial { get; set; } = "";
    public string? Marca { get; set; }
    public string? Medida { get; set; }
}
