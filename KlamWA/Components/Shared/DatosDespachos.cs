namespace KlamWA.Components.Shared;

public sealed record DespachoVista(
    string Codigo, int IdCirugia, string Equipo, string Kit,
    DateTime Salida, DateTime? Retorno);

public static class DatosDespachos
{
    public static IReadOnlyList<DespachoVista> Lista { get; } = Array.AsReadOnly<DespachoVista>([
        new("DSP-001", 1, "Artroscopio", "Bandeja Artroscopia", new(2026, 10, 5, 6, 30, 0), new(2026, 10, 5, 14, 0, 0)),
        new("DSP-003", 3, "Taladro", "Bandeja Trauma", new(2026, 10, 26, 6, 30, 0), null),
        new("DSP-005", 5, "Taladro", "Bandeja Columna", new(2026, 10, 30, 8, 0, 0), null),
        new("DSP-006", 6, "Taladro", "Bandeja Columna", new(2026, 10, 20, 6, 0, 0), new(2026, 10, 20, 15, 0, 0)),
        new("DSP-007", 7, "Taladro", "Bandeja Cráneo", new(2026, 10, 22, 7, 0, 0), null)
    ]);

    public static DespachoVista? De(string codigo) =>
        Lista.FirstOrDefault(d => d.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
}
