namespace KlamWA.DTOs;

// Espejo de pe.edu.pucp.klam.rest.dto.* del reporte de consumo (RF18 y RF19).
// Las fechas llegan como texto ISO (AAAA-MM-DDTHH:mm:ss) y se leen como DateTime.

public class ReporteUsoInsumosDTO
{
    public DateTime? Generado { get; set; }
    public string? Desde { get; set; }
    public string? Hasta { get; set; }
    public string? TipoCirugia { get; set; }
    public string? Medida { get; set; }
    public string? Consumible { get; set; }
    public int CirugiasConsideradas { get; set; }
    public List<ResumenUsoInsumoDTO> Insumos { get; set; } = [];
}

public class ResumenUsoInsumoDTO
{
    public string Nombre { get; set; } = "";
    public string? Medida { get; set; }
    public string? TipoCirugia { get; set; }
    public int CantidadUtilizada { get; set; }
    public int Frecuencia { get; set; }
}

public class ReporteComparativoDTO
{
    public DateTime? Generado { get; set; }
    public string? Desde { get; set; }
    public string? Hasta { get; set; }
    public int? IdCirugia { get; set; }
    public string? TipoCliente { get; set; }
    public int? IdCliente { get; set; }
    public List<CirugiaComparativaDTO> Cirugias { get; set; } = [];
}

public class CirugiaComparativaDTO
{
    public int IdCirugia { get; set; }
    public DateTime? Fecha { get; set; }
    public string? Procedimiento { get; set; }
    public string? Paciente { get; set; }
    public string? DocumentoPaciente { get; set; }
    public string? Medico { get; set; }
    public string? TipoCliente { get; set; }
    public string? Cliente { get; set; }
    public List<LineaComparativaDTO> Insumos { get; set; } = [];
}

public class LineaComparativaDTO
{
    public string Nombre { get; set; } = "";
    public string? Medida { get; set; }
    public int Despachada { get; set; }
    public int Consumida { get; set; }
    public int Diferencia { get; set; }
}

/// Criterios de RF18. Los nulos no filtran.
public sealed record FiltroUsoInsumos(DateOnly? Desde, DateOnly? Hasta, string? TipoCirugia,
    int? IdConsumible, string? Medida);

/// Criterios de RF19. Los nulos no filtran.
public sealed record FiltroComparativo(DateOnly? Desde, DateOnly? Hasta, int? IdCirugia,
    string? TipoCliente, int? IdCliente);

/// Archivo generado por el backend (Excel o PDF), listo para descargar.
public sealed class ArchivoReporteDTO
{
    public string Nombre { get; set; } = "reporte";
    public byte[] Contenido { get; set; } = [];
    public string TipoContenido { get; set; } = "application/octet-stream";
}