namespace KlamWA.Dtos;

// Cliente para selectores (Java: ClienteResumenDTO). Nunca trae credenciales.
public class ClienteResumenDto
{
    public string TipoCliente { get; set; } = "";   // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = "";
    public string Documento { get; set; } = "";
    public string? Dni { get; set; }
    public string? EmailContacto { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; }
}
