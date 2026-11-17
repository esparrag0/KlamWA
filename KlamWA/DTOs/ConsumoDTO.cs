namespace KlamWA.DTOs;

// Espejo de ConsumoCirugiaDTO (Java): fila del listado GET /consumos.
// EstadoOrdenCompra: NO_APLICA | POR_ADJUNTAR | PENDIENTE | VALIDADO | RECHAZADO
// EstadoConsumo:     NO_DISPONIBLE | POR_REGISTRAR | COMPLETO
public class ConsumoCirugiaDTO
{
    public int IdCirugia { get; set; }
    public string? FechaHoraInicio { get; set; }
    public string? TipoProcedimiento { get; set; }
    public string TipoCliente { get; set; } = "";
    public string? ClienteNombre { get; set; }
    public int? IdBandeja { get; set; }
    public bool RequiereOrdenCompra { get; set; }
    public string EstadoOrdenCompra { get; set; } = "";
    public bool DocumentacionCierreCompleta { get; set; }
    public string EstadoConsumo { get; set; } = "";
}

// Espejo de ConsumoDetalleDTO (Java): GET /consumos/{idCirugia}.
public class ConsumoDetalleDTO : ConsumoCirugiaDTO
{
    public string? DoctorNombre { get; set; }
    public string? PacienteNombre { get; set; }
    public string? ClinicaNombre { get; set; }
    public string? ArchivoOrdenCompra { get; set; }
    public string? MotivoRechazoOrdenCompra { get; set; }
    public int TotalDespachado { get; set; }
    public int TotalConsumido { get; set; }
    public int Diferencia { get; set; }
    public List<ConsumoLineaDTO> Consumibles { get; set; } = [];
}

public class ConsumoLineaDTO
{
    public int IdConsumible { get; set; }
    public string Nombre { get; set; } = "";
    public string Medida { get; set; } = "";
    public int CantidadDespachada { get; set; }
    public int CantidadConsumida { get; set; }
    public int Diferencia { get; set; }
}

// Cuerpo de PUT /consumos/{idCirugia}. Con Confirmar = true el consumo queda completo y ya no se puede editar.
public class RegistroConsumoDTO
{
    public int IdCirugia { get; set; }
    public bool Confirmar { get; set; }
    public List<LineaRegistroConsumoDTO> Lineas { get; set; } = [];
}

public class LineaRegistroConsumoDTO
{
    public int IdConsumible { get; set; }
    public int CantidadConsumida { get; set; }
}
