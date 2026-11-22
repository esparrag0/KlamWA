namespace KlamWA.DTOs;

// Datos de una clínica que aún no está registrada: el vendedor los envía junto con la solicitud.
public sealed class ClinicaNuevaDTO
{
    public string Nombre { get; set; } = string.Empty;
    public string Ruc { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string EmailContacto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? PeriodoCredito { get; set; }
    public bool TieneConsignacion { get; set; }
}
