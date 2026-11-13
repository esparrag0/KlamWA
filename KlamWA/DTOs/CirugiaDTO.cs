using System.Globalization;
using System.Text.Json.Serialization;

namespace KlamWA.DTOs;

// Cirugía tal como la devuelve GET /cirugias/{id} (fechas en texto ISO)
public class CirugiaDTO
{
    public int IdCirugia { get; set; }
    public string Estado { get; set; } = "";
    public string? FechaHoraInicio { get; set; }
    public string? FechaHoraFin { get; set; }
    public string TipoProcedimiento { get; set; } = "";
    public string DoctorNombre { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public string? MotivoCancelacion { get; set; }
    public string TipoCliente { get; set; } = "";
    public int? IdCliente { get; set; }
    public string? ClienteNombre { get; set; }
    public string? PacienteNombre { get; set; }
    public string? ClinicaNombre { get; set; }
    public int? IdEquipo { get; set; }
    public string? EquipoNombre { get; set; }
    public int? IdBandeja { get; set; }
    public Dictionary<int, int> ConsumiblesPrevistos { get; set; } = [];
    public string? BandejaTipo { get; set; }
    public int? IdSolicitudOrigen { get; set; }
    public double? PrecioPactado { get; set; }
    public string? EstadoCotizacion { get; set; }
    public bool Activo { get; set; }

    [JsonIgnore] public DateTime? Inicio => Fecha(FechaHoraInicio);
    [JsonIgnore] public DateTime? Fin => Fecha(FechaHoraFin);

    // Texto que se muestra en pantalla (mismos nombres que el Design System)
    [JsonIgnore]
    public string EstadoTexto => Estado switch
    {
        "PROGRAMADA" => "Programada",
        "EN_PROCESO" => "En curso",
        "FINALIZADA" => "Culminada",
        "CANCELADA" => "Cancelada",
        _ => Estado
    };

    private static DateTime? Fecha(string? valor) =>
        DateTime.TryParse(valor, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime fecha) ? fecha : null;
}

// Cuerpo de POST /cirugias y PUT /cirugias/{id}
public class CirugiaFormDTO
{
    public Dictionary<int, int> ConsumiblesPrevistos { get; set; } = [];
    public string? FechaHoraInicio { get; set; }
    public string? FechaHoraFin { get; set; }
    public string TipoProcedimiento { get; set; } = "";
    public string DoctorNombre { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public string TipoCliente { get; set; } = "";
    public int IdCliente { get; set; }
    public int? IdEquipo { get; set; }
    public int? IdBandeja { get; set; }
}

// Cuerpo de /estado y /cancelacion
public class AccionCirugiaDTO
{
    public string? Estado { get; set; }
    public string? Motivo { get; set; }
}

public class OpcionDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string? Tipo { get; set; }
}

// Listas reales para el formulario de cirugía
public class OpcionesCirugiaDTO
{
    public List<string> Procedimientos { get; set; } = [];
    public List<OpcionDTO> Clientes { get; set; } = [];
    public List<OpcionDTO> Equipos { get; set; } = [];
    public List<BandejaDTO> Bandejas { get; set; } = [];
}
