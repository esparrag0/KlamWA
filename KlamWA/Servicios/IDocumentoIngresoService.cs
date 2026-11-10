using KlamWA.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace KlamWA.Servicios;

public interface IDocumentoIngresoService
{
    Task<List<DocumentoIngresoDTO>> ListarPorSolicitudAsync(int idSolicitud);
    Task<List<DocumentoIngresoDTO>> ListarPropiosAsync(int? idSolicitud = null);
    Task RegistrarArchivoAsync(int idSolicitud, string tipoDocumento, IBrowserFile archivo);
    Task ResubirArchivoAsync(int idDocumento, IBrowserFile archivo);
    Task<ArchivoDocumentoDTO> DescargarAsync(int idDocumento);
    Task<List<HistorialDocumentoDTO>> ConsultarHistorialAsync(int idSolicitud);
    Task<VerificacionDocumentosDTO> VerificarDocumentosAsync(int idSolicitud);
    Task ValidarAsync(int idDocumento);
    Task RechazarAsync(int idDocumento, string motivo, bool habilitarResubida);
}
