using KlamWA.ViewModels;

namespace KlamWA.Components.Shared;

// TEMPORAL: permite que pantallas de otros integrantes que aún usan CirugiaMock sigan mostrando
// TablaCirugias (que ahora recibe CirugiaResumenViewModel). Borrar cuando esas pantallas usen los servicios.
public static class CirugiaMockExtensions
{
    public static CirugiaResumenViewModel ADetalleVista(this CirugiaMock c) => new()
    {
        Id = c.Id, SolicitudId = c.SolicitudId, Inicio = c.Inicio, Fin = c.Fin,
        Procedimiento = c.Procedimiento, Doctor = c.Doctor, Paciente = c.Paciente,
        Diagnostico = c.Diagnostico, Clinica = c.Clinica, Cliente = c.Cliente,
        Equipo = c.Equipo, Bandeja = c.Bandeja, Estado = c.Estado,
        MotivoCancelacion = c.MotivoCancelacion, PrecioCotizacion = c.PrecioCotizacion,
        EstadoCotizacion = c.EstadoCotizacion
    };
}
