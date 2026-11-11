using KlamWA.DTOs;

namespace KlamWA.Servicios;

// Gestión comercial y habilitación independiente de una cuenta por el administrador.
public interface IGestionClienteService
{
    Task<List<ClienteGestionDTO>> ListarAsync(string? tipoCliente = null);
    Task<ClienteGestionDTO> RegistrarAsync(ClienteFormDTO cliente);
    Task<ClienteFormDTO> ObtenerAsync(string tipoCliente, int id);
    Task ActualizarAsync(string tipoCliente, int id, ClienteFormDTO cliente);
    Task HabilitarCuentaAsync(string tipoCliente, int id, string username, string password);
}
