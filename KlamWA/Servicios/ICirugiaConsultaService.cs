using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ICirugiaConsultaService
{
    /// <summary>Cirugías culminadas y canceladas del cliente de la sesión (RF10).</summary>
    Task<List<CirugiaHistorialDTO>> HistorialClienteAsync();

    /// <summary>Historial de un cliente elegido por el administrador.</summary>
    Task<List<CirugiaHistorialDTO>> HistorialDeClienteAsync(string tipoCliente, int idCliente);
}
