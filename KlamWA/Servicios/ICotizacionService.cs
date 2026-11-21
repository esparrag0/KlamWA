using KlamWA.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace KlamWA.Servicios;

public interface ICotizacionService
{
    Task<List<CotizacionDTO>> ListarAsync(int idSolicitud);
    Task<CotizacionDTO?> VigenteAsync(int idSolicitud);
    Task<CotizacionCreadaDTO> AdjuntarAsync(int idSolicitud, decimal precioPactado, IBrowserFile archivo);
    Task<ArchivoDocumentoDTO> DescargarAsync(int idCotizacion);
    Task DecidirAsync(int idCotizacion, bool aceptar, string? motivo);
    Task<ElegibilidadCirugiaDTO> ElegibilidadAsync(int idSolicitud);
}