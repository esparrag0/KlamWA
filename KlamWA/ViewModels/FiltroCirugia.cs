namespace KlamWA.ViewModels;

/// Filtros de búsqueda de cirugías (RF17). Todos son opcionales y se combinan. IdCliente requiere TipoCliente.
public sealed record FiltroCirugia(
    DateTime? Desde = null,
    DateTime? Hasta = null,
    string? TipoCliente = null,
    int? IdCliente = null,
    int? IdEquipo = null,
    string? Estado = null);
