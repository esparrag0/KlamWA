using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface IBandejaService
{
    Task<List<BandejaDTO>> ListarAsync(string? procedimiento);
    Task<BandejaDTO?> ObtenerAsync(int id);
    Task<BandejaDTO> RegistrarAsync(BandejaFormDTO bandeja);
    Task<BandejaDTO> AsociarConsumibleAsync(int idBandeja, int idConsumible, int cantidad);
    Task<BandejaDTO> ActualizarCantidadAsync(int idBandeja, int idConsumible, int cantidad);
    Task<BandejaDTO> QuitarConsumibleAsync(int idBandeja, int idConsumible);
    Task<List<ConsumibleDTO>> ListarConsumiblesAsync();
}
