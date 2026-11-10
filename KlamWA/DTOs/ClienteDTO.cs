namespace KlamWA.DTOs;

// Espejo de ClienteDTO (Java). TipoCliente: CLINICA_HOSPITAL o PACIENTE_PARTICULAR.
// Los ids de clínicas y pacientes se solapan: un cliente se identifica por TipoCliente + IdCliente.
public class ClienteDTO
{
    public int IdCliente { get; set; }
    public bool Activo { get; set; }
    public string TipoCliente { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string? Direccion { get; set; }
    public string? EmailContacto { get; set; }
    public string? Telefono { get; set; }
    // Clínica u hospital
    public string? Ruc { get; set; }
    public bool TieneConsignacion { get; set; }
    public string? PeriodoCredito { get; set; }
    // Paciente particular
    public string? Dni { get; set; }
    public bool PagoConfirmado { get; set; }
}
