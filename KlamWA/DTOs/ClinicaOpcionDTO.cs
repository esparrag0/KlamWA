namespace KlamWA.DTOs;

// Opción del buscador de clínicas por nombre (Java: GET cliente/solicitudes/clinicas?nombre=). Solo id y nombre.
public sealed class ClinicaOpcionDTO
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
