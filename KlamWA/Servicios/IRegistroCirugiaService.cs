using KlamWA.DTOs;

namespace KlamWA.Servicios;

// RF09: el administrador registra una cirugía (POST admin/cirugias, solo administrador verificado en el servidor).
// El CRUD completo de cirugías (RF02) usa el recurso cirugias y es de otro integrante.
public interface IRegistroCirugiaService
{
    Task<CirugiaRegistradaDTO> RegistrarAsync(CirugiaRegistroDTO cirugia);
}
