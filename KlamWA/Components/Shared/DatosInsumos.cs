namespace KlamWA.Components.Shared;

// Muestras de la presentación. Despachado y consumido solo se muestran en
// cirugías culminadas; en las demás la cantidad consumida está pendiente.
public sealed record InsumoCirugiaVista(string Nombre, string Medida, int Despachado, int? Consumido)
{
    public int? Diferencia => Consumido is int real ? Despachado - real : null;
}

public sealed record PlantillaBandejaVista(string Nombre, string Procedimiento, IReadOnlyList<InsumoCirugiaVista> Insumos);

public static class DatosInsumos
{
    public static IReadOnlyList<PlantillaBandejaVista> Plantillas { get; } = Array.AsReadOnly<PlantillaBandejaVista>([
        new("Bandeja Cráneo", "Cirugía de cráneo/cabeza", [
            new("Fresa craneal", "65 mm", 3, null),
            new("Cuchilla", "70 mm", 2, null)
        ]),
        new("Bandeja Columna", "Cirugía de columna", [
            new("Tornillo pedicular", "6 mm", 4, null),
            new("Fresa cortante", "70 mm", 2, null)
        ])
    ]);

    private static readonly IReadOnlyDictionary<int, IReadOnlyList<InsumoCirugiaVista>> PorCirugia =
        new Dictionary<int, IReadOnlyList<InsumoCirugiaVista>>
        {
            [1] = [new("Fresa cortante", "70 mm", 4, 2), new("Cuchilla corta", "70 mm", 2, 1)],
            [3] = [new("Tornillo cortical", "3.5 mm", 6, null)],
            [4] = [new("Cuchilla corta", "70 mm", 2, null)],
            [5] = [new("Tornillo pedicular", "6 mm", 4, null)],
            [6] = [new("Tornillo pedicular", "6 mm", 4, null)],
            [7] = [new("Fresa craneal", "65 mm", 3, null)],
            [8] = [new("Tornillo pedicular", "6 mm", 4, null), new("Fresa cortante", "70 mm", 2, null)]
        };

    public static IReadOnlyList<InsumoCirugiaVista> DeCirugia(int id) =>
        PorCirugia.TryGetValue(id, out var items) ? items : [];
}
