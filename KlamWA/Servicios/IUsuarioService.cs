using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface IUsuarioService
{
    Task<List<UsuarioDTO>> ListarAsync(string? rol = null, string? estado = null, string? busqueda = null);
    Task<MetricasUsuariosDTO> MetricasAsync();
    Task<List<UsuarioDTO>> ListarInstrumentistasActivosAsync();
    Task<UsuarioDTO?> ObtenerPorIdAsync(int id, string? rol = null);
    Task<UsuarioDTO> CrearAsync(UsuarioFormDTO dto);
    Task<UsuarioDTO> ModificarAsync(int id, UsuarioFormDTO dto, string? rol = null);
    Task EliminarAsync(int id, string? rol = null);
    Task ReactivarAsync(int id, string? rol = null);
}
