namespace KlamWA.DTOs;

// Cliente para listas y selectores (Java: ClienteResumenDTO de GET gestion-clientes). Nunca trae credenciales.
public sealed class ClienteGestionDTO
{
    public string TipoCliente { get; set; } = string.Empty;   // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;     // "RUC ..." o "DNI ..."
    public string? Dni { get; set; }
    public string? EmailContacto { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; }
}
