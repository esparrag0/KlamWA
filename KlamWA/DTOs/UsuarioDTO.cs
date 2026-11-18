namespace KlamWA.DTOs;

public class UsuarioDTO
{
    public int Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;
    public string? Especialidad { get; set; }
    public double? ComisionAcumulada { get; set; }
    public string Iniciales { get; set; } = string.Empty;
    public int? IdCliente { get; set; }
    public string? TipoCliente { get; set; }
    public bool EsCliente => TipoCliente != null;
}

public class UsuarioFormDTO
{
    public int? Id { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = "Vendedor";
    public bool Activo { get; set; } = true;
    public string? Especialidad { get; set; }
    public string? Password { get; set; }
    public double? ComisionAcumulada { get; set; }
}

public class MetricasUsuariosDTO
{
    public int TotalRegistrados { get; set; }
    public int CuentasActivas { get; set; }
    public int CuentasInactivas { get; set; }
    public int RolesPersonal { get; set; }
}
