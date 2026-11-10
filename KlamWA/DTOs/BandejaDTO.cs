namespace KlamWA.DTOs;

// Bandeja (plantilla) con sus consumibles, según GET /bandejas
public class BandejaDTO
{
    public int IdBandeja { get; set; }
    public string Tipo { get; set; } = "";
    public string? Procedimiento { get; set; }
    public bool Esterilizado { get; set; }
    public bool Activo { get; set; }
    public int? IdCirugia { get; set; }
    public List<ConsumibleBandejaDTO> Consumibles { get; set; } = [];
}

public class ConsumibleBandejaDTO
{
    public int IdConsumible { get; set; }
    public string? NombreComercial { get; set; }
    public string? Medida { get; set; }
    public int Cantidad { get; set; }
}

// Cuerpo de POST /bandejas
public class BandejaFormDTO
{
    public string Tipo { get; set; } = "";
    public string Procedimiento { get; set; } = "";
    public bool Esterilizado { get; set; } = true;
}

public class ConsumibleDTO
{
    public int IdConsumible { get; set; }
    public string NombreComercial { get; set; } = "";
    public string? Marca { get; set; }
    public string? Medida { get; set; }
    public bool Activo { get; set; }
}
