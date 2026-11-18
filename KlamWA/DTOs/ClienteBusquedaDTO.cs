namespace KlamWA.DTOs;

public sealed class ClienteBusquedaDTO
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Dni { get; set; }
    public string? Ruc { get; set; }
}
