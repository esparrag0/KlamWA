using KlamWA.DTOs;
using Microsoft.AspNetCore.Components.Forms;

namespace KlamWA.Servicios;

public interface IDocumentoIngresoService
{
    Task<List<DocumentoIngresoDTO>> ListarPorSolicitudAsync(int idSolicitud);

    /// <summary>Documentos de una cirugía (administrador o el cliente dueño).</summary>
    Task<List<DocumentoIngresoDTO>> ListarPorCirugiaAsync(int idCirugia);

    /// <summary>
    /// RF14/RF15: adjunta la orden de compra de una cirugía culminada. Solo clínicas
    /// y hospitales; el backend rechaza cualquier otro caso.
    /// </summary>
    Task AdjuntarOrdenCompraAsync(int idCirugia, IBrowserFile archivo);
    Task<List<DocumentoIngresoDTO>> ListarPropiosAsync(int? idSolicitud = null);
    Task RegistrarArchivoAsync(int idSolicitud, string tipoDocumento, IBrowserFile archivo);
    Task ResubirArchivoAsync(int idDocumento, IBrowserFile archivo);
    Task<ArchivoDocumentoDTO> DescargarAsync(int idDocumento);
    Task<List<HistorialDocumentoDTO>> ConsultarHistorialAsync(int idSolicitud);
    Task<VerificacionDocumentosDTO> VerificarDocumentosAsync(int idSolicitud);
    Task ValidarAsync(int idDocumento);
    Task RechazarAsync(int idDocumento, string motivo, bool habilitarResubida);

    /// <summary>Documentos obligatorios y versión vigente de cada uno (cliente dueño, vendedor o administrador).</summary>
    Task<RequisitosDocumentosDTO> RequisitosAsync(int idSolicitud);
}
