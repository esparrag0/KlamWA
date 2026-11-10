namespace KlamWA.DTOs;

// Espejo de LineaConsumibleDTO (Java): un consumible con su cantidad dentro de una bandeja.
public class LineaConsumibleDTO
{
    public ConsumibleDTO Consumible { get; set; } = new();
    public int Cantidad { get; set; }
}
