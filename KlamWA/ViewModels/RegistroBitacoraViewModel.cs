using KlamWA.DTOs;

namespace KlamWA.ViewModels {
    public class RegistroBitacoraViewModel
    {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        public string Cirugia { get; set; } = "";

        public string Accion { get; set; } = "";
        public string AccionCodigo { get; set; } = "";
        public string Responsable { get; set; } = "";

        public string Detalle { get; set; } = "";

        public static readonly IReadOnlyDictionary<string, string> Acciones = new Dictionary<string, string>
        {
            ["CREAR"] = "Creación",
            ["EDITAR"] = "Modificación",
            ["CAMBIAR_ESTADO"] = "Cambio de estado",
            ["CANCELAR"] = "Cancelación",
            ["ELIMINAR"] = "Eliminación",
            ["ASIGNAR_EQUIPO"] = "Asignación de equipo",
            ["CAMBIAR_EQUIPO"] = "Cambio de equipo",
            ["ASIGNAR_BANDEJA"] = "Asignación de bandeja",
            ["CAMBIAR_BANDEJA"] = "Cambio de bandeja",
        };

        public static RegistroBitacoraViewModel DesdeDTO(AuditoriaDTO dto) => new()
        {
            Id = dto.IdAuditoria,
            FechaHora = dto.FechaHora,
            Cirugia = $"CIR-{dto.IdCirugia:000}",
            AccionCodigo = dto.Accion,
            Accion = Acciones.GetValueOrDefault(dto.Accion, dto.Accion),
            Responsable = dto.Responsable,
            Detalle = dto.Detalle ?? "",
        };
    }
    }
