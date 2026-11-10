namespace KlamWA.DTOs;

// Espejo de CirugiaDetalleDTO (Java): el resumen más el equipo (con marca) y la bandeja con sus consumibles.
public class CirugiaDetalleDTO : CirugiaResumenDTO
{
    public EquipoDTO? Equipo { get; set; }
    public string? EquipoMarca { get; set; }
    public BandejaInstrumentalDTO? BandejaInstrumental { get; set; }
}
