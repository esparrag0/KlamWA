namespace KlamWA.DTOs;

// Espejo de EquipoDTO (Java). Categoria: CRANEOTOMO, NAVEGADOR, MICROSCOPIO u OTRO.
// Especificaciones es clave/valor libre; la marca viene en la clave "marca".
public class EquipoDTO
{
    public int IdEquipo { get; set; }
    public bool Activo { get; set; }
    public string Nombre { get; set; } = "";
    public string? Categoria { get; set; }
    public Dictionary<string, string> Especificaciones { get; set; } = new();
    public bool Disponible { get; set; }
}
