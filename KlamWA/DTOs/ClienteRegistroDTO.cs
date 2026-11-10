namespace KlamWA.DTOs;

public sealed class ClienteRegistroDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string EmailContacto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? Dni { get; set; }
    public string? Ruc { get; set; }
    public bool TieneConsignacion { get; set; }
    public string? PeriodoCredito { get; set; }
}
