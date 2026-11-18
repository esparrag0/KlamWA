using KlamWA.DTOs;

namespace KlamWA.Servicios;

// Gestión comercial y habilitación independiente de una cuenta por el administrador.
public interface IGestionClienteService
{
    Task<List<ClienteGestionDTO>> ListarAsync(string? tipoCliente = null);
    Task<ClienteGestionDTO> RegistrarAsync(ClienteFormDTO cliente);
    Task<ClienteFormDTO> ObtenerAsync(string tipoCliente, int id);
    Task ActualizarAsync(string tipoCliente, int id, ClienteFormDTO cliente);
    /// <summary>
    /// Habilita la cuenta de acceso del cliente. El backend genera la contraseña
    /// temporal y se la envía por correo; la devuelve una sola vez para mostrarla.
    /// </summary>
    Task<CredencialClienteDTO> HabilitarCuentaAsync(string tipoCliente, int id, string username);
}
