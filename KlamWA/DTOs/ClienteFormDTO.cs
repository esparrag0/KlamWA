namespace KlamWA.DTOs;

// Datos comerciales de alta, detalle y modificación en gestion-clientes.
public sealed class ClienteFormDTO
{
    public int IdCliente { get; set; }
    public bool Activo { get; set; } = true;
    public string TipoCliente { get; set; } = string.Empty;   // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    public string Nombre { get; set; } = string.Empty;
    public string? Direccion { get; set; }
    public string? EmailContacto { get; set; }
    public string? Telefono { get; set; }
    public string? Ruc { get; set; }
    public bool TieneConsignacion { get; set; }
    public string? PeriodoCredito { get; set; }
    public string? Dni { get; set; }
    public bool PagoConfirmado { get; set; }
}
