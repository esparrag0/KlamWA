using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface IDespachoService
{
    Task<List<DespachoDTO>> ListarAsync(string? estado = null, string? busqueda = null);
    Task<List<CirugiaDespachableDTO>> CirugiasDespachablesAsync();
    Task<DespachoDetalleDTO?> ObtenerAsync(string codigo);
    Task<string> RegistrarAsync(NuevoDespachoDTO despacho);
    Task RegistrarRetornoAsync(string codigo);
}
