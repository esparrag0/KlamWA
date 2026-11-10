namespace KlamWA.DTOs;

// Alta (POST), detalle (GET) y modificación (PUT) de gestion-clientes (Java: ClienteRegistroDTO).
// Username y Password solo se usan en el alta; la contraseña nunca viaja en las respuestas.
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
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
