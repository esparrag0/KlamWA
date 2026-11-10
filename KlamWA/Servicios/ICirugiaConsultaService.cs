using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface ICirugiaConsultaService
{
    /// <summary>Cirugias culminadas y canceladas de un cliente.</summary>
    /// <param name="tipoCliente">CLINICA_HOSPITAL o PACIENTE_PARTICULAR.</param>
    /// <param name="idCliente">Identificador del cliente en la BD.</param>
    Task<List<CirugiaHistorialDTO>> HistorialClienteAsync(string tipoCliente, int idCliente);
}
