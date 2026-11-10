namespace KlamWA.Dtos;

// POST clientes (Java: ClienteRegistroDTO). Alta de cliente CON usuario y contraseña (RF01).
public class ClienteRegistroDto
{
    public int IdCliente { get; set; }               // solo en el detalle
    public bool Activo { get; set; } = true;
    public string TipoCliente { get; set; } = "";   // CLINICA_HOSPITAL | PACIENTE_PARTICULAR
    public string Nombre { get; set; } = "";
    public string? Direccion { get; set; }
    public string? EmailContacto { get; set; }
    public string? Telefono { get; set; }
    public string? Ruc { get; set; }
    public bool TieneConsignacion { get; set; }
    public string? PeriodoCredito { get; set; }
    public string? Dni { get; set; }
    public bool PagoConfirmado { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
