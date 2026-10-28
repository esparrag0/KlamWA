using KlamWA.ViewModels;

namespace KlamWA.Components.Shared;

// Bitácora ficticia referida a las mismas cirugías de DatosAgenda.
public static class DatosBitacora
{
    public static IReadOnlyList<RegistroBitacoraViewModel> Registros { get; } =
        Array.AsReadOnly<RegistroBitacoraViewModel>([
            new() { Id=1, FechaHora=new(2026,10,3,9,20,0), Cirugia="CIR-001", Accion="Creación", Responsable="Valeria Torres", Detalle="Alta de programación" },
            new() { Id=2, FechaHora=new(2026,10,25,10,10,0), Cirugia="CIR-003", Accion="Modificación", Responsable="Valeria Torres", Detalle="Horario y equipo modificados" },
            new() { Id=3, FechaHora=new(2026,10,11,11,40,0), Cirugia="CIR-002", Accion="Cancelación", Responsable="Valeria Torres", Detalle="Paciente no apto" },
            new() { Id=4, FechaHora=new(2026,10,26,8,5,0), Cirugia="CIR-004", Accion="Creación", Responsable="Lucía Salazar", Detalle="Alta de programación" },
            new() { Id=5, FechaHora=new(2026,10,26,9,15,0), Cirugia="CIR-005", Accion="Modificación", Responsable="Lucía Salazar", Detalle="Bandeja modificada" }
        ]);

    public static IReadOnlyList<CambioCampoViewModel> Cambios(int id) => id switch
    {
        1 or 4 => [
            new() { Campo="Estado", ValorAnterior="—", ValorPosterior="Programada" },
            new() { Campo="Fecha y hora de inicio", ValorAnterior="—", ValorPosterior=id == 1 ? "05/10/2026 08:00" : "28/10/2026 14:00" }
        ],
        3 => [
            new() { Campo="Estado", ValorAnterior="Programada", ValorPosterior="Cancelada" },
            new() { Campo="Motivo de cancelación", ValorAnterior="—", ValorPosterior="Paciente no apto" }
        ],
        2 => [
            new() { Campo="Hora de inicio", ValorAnterior="08:00", ValorPosterior="09:00" },
            new() { Campo="Equipo", ValorAnterior="Sierra", ValorPosterior="Taladro" }
        ],
        5 => [new() { Campo="Bandeja", ValorAnterior="Bandeja Trauma", ValorPosterior="Bandeja Columna" }],
        _ => []
    };
}
