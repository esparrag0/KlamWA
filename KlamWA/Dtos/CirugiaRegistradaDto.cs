namespace KlamWA.Dtos;

// Respuesta de POST cirugias: solo lo que usa la pantalla (la BL devuelve el detalle completo).
public class CirugiaRegistradaDto
{
    public int IdCirugia { get; set; }
    public string? Estado { get; set; }
}
