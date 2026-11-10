using System.Globalization;
using System.Text.Json.Serialization;

namespace KlamWA.DTOs;

public class DespachoDTO
{
    public int IdDespacho { get; set; }
    public string Codigo { get; set; } = "";
    public int IdCirugia { get; set; }
    public string? Cliente { get; set; }
    public string? Clinica { get; set; }
    public string? Procedimiento { get; set; }
    public int? IdEquipo { get; set; }
    public string? Equipo { get; set; }
    public int? IdBandeja { get; set; }
    public string? Kit { get; set; }
    public string? FechaSalida { get; set; }
    public string? FechaRetorno { get; set; }
    public string Estado { get; set; } = "";
    public bool PendienteRetorno { get; set; }

    [JsonIgnore]
    public DateTime? Salida => DateTime.TryParse(FechaSalida, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;

    [JsonIgnore]
    public DateTime? Retorno => DateTime.TryParse(FechaRetorno, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;
}

public class DespachoDetalleDTO : DespachoDTO
{
    public string? Paciente { get; set; }
    public string? Doctor { get; set; }
    public string? FechaCirugia { get; set; }
    public string? EstadoCirugia { get; set; }
    public List<ConsumibleDespachadoDTO> Consumibles { get; set; } = [];

    [JsonIgnore]
    public DateTime? CirugiaFecha => DateTime.TryParse(FechaCirugia, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;
}

public class ConsumibleDespachadoDTO
{
    public int? IdConsumible { get; set; }
    public string Nombre { get; set; } = "";
    public string Medida { get; set; } = "";
    public int CantidadDespachada { get; set; }

    [JsonIgnore]
    public int Despachado => CantidadDespachada;
}

public class CirugiaDespachableDTO
{
    public int IdCirugia { get; set; }
    public string Procedimiento { get; set; } = "";
    public string Cliente { get; set; } = "";
    public string Clinica { get; set; } = "";
    public string? FechaHoraInicio { get; set; }
    public string Estado { get; set; } = "";
    public int? IdEquipo { get; set; }
    public string? Equipo { get; set; }
    public bool? EquipoDisponible { get; set; }
    public int? IdBandeja { get; set; }
    public string? Kit { get; set; }
    public bool Despachable { get; set; }
    public string? Motivo { get; set; }

    [JsonIgnore]
    public DateTime? Inicio => DateTime.TryParse(FechaHoraInicio, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : null;
}

public class NuevoDespachoDTO
{
    public int IdCirugia { get; set; }
}

public class DespachoRespuestaDTO
{
    public string Codigo { get; set; } = "";
}
