using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ICirugiaConsultaService
{
    /// <summary>Cirugias culminadas y canceladas de un cliente.</summary>
    Task<List<CirugiaHistorialDTO>> HistorialClienteAsync();
}
