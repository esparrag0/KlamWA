using KlamWA.DTOs;

namespace KlamWA.Servicios;

public interface IAuditoriaDocumentalService
{
    Task<List<AuditoriaDocumentalDTO>> BuscarAsync(string? cliente, string? paciente,
        DateTime? desde, DateTime? hasta, string? tipoDocumento, string? accion,
        string? responsable, int? idSolicitud, int? idCirugia, int pagina, int tamano);

    Task<AuditoriaDocumentalDTO?> ObtenerAsync(int idEvento);
}
