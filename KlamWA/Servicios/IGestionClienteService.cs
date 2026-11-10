using KlamWA.DTOs;

namespace KlamWA.Servicios;

// Gestión de clientes por el administrador (alta con usuario y contraseña, detalle, modificación) y listas para selectores.
public interface IGestionClienteService
{
    Task<List<ClienteGestionDTO>> ListarAsync(string? tipoCliente = null);
    Task<ClienteGestionDTO> RegistrarAsync(ClienteFormDTO cliente);
    Task<ClienteFormDTO> ObtenerAsync(string tipoCliente, int id);
    Task ActualizarAsync(string tipoCliente, int id, ClienteFormDTO cliente);
}
