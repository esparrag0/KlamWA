namespace KlamWA.Components.Shared;

// Solo datos ficticios para Lab 9 (sin BL, sin BD, sin servicios).
// Campos alineados con la clase Java Cirugia / Cotizacion.
// Estados en pantalla: Programada (PROGRAMADA), En curso (EN_PROCESO),
// Culminada (FINALIZADA), Cancelada (CANCELADA).
public class CirugiaMock
{
    public int Id { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime Fin { get; set; }
    public string Procedimiento { get; set; } = "";
    public string Doctor { get; set; } = "";
    public string Cliente { get; set; } = "";       // clínica/hospital o paciente particular
    public string Equipo { get; set; } = "";
    public string Bandeja { get; set; } = "";
    public string Estado { get; set; } = "";
    public string MotivoCancelacion { get; set; } = "";
    public double PrecioCotizacion { get; set; }
    public string EstadoCotizacion { get; set; } = ""; // Emitida, Aceptada, Rechazada, Vencida
}

public static class DatosAgenda
{
    public static List<CirugiaMock> Cirugias = new()
    {
        new() { Id=1, Inicio=new DateTime(2026,10,5,8,0,0),   Fin=new DateTime(2026,10,5,10,0,0),  Procedimiento="Artroscopia de rodilla", Doctor="Dr. Ramos", Cliente="Clínica San Felipe",   Equipo="Artroscopio", Bandeja="Bandeja Artroscopia", Estado="Culminada", PrecioCotizacion=4500, EstadoCotizacion="Aceptada" },
        new() { Id=2, Inicio=new DateTime(2026,10,12,10,30,0),Fin=new DateTime(2026,10,12,13,0,0), Procedimiento="Reemplazo de cadera",    Doctor="Dra. Lima", Cliente="Hospital Rebagliati", Equipo="Sierra",      Bandeja="Bandeja Cadera",      Estado="Cancelada", MotivoCancelacion="Paciente no apto", PrecioCotizacion=8200, EstadoCotizacion="Rechazada" },
        new() { Id=3, Inicio=new DateTime(2026,10,26,9,0,0),  Fin=new DateTime(2026,10,26,12,0,0), Procedimiento="Osteosíntesis de tibia", Doctor="Dr. Ramos", Cliente="Clínica San Felipe",   Equipo="Taladro",     Bandeja="Bandeja Trauma",      Estado="En curso",  PrecioCotizacion=6000, EstadoCotizacion="Aceptada" },
        new() { Id=4, Inicio=new DateTime(2026,10,28,14,0,0), Fin=new DateTime(2026,10,28,16,0,0), Procedimiento="Artroscopia de hombro", Doctor="Dr. Vega",  Cliente="Luis Rojas (particular)", Equipo="Artroscopio", Bandeja="Bandeja Artroscopia", Estado="Programada", PrecioCotizacion=4800, EstadoCotizacion="Emitida" },
        new() { Id=5, Inicio=new DateTime(2026,10,30,11,0,0), Fin=new DateTime(2026,10,30,15,0,0), Procedimiento="Cirugía de columna",    Doctor="Dra. Lima", Cliente="Clínica Anglo",       Equipo="Taladro",     Bandeja="Bandeja Columna",     Estado="Programada", PrecioCotizacion=12000, EstadoCotizacion="Emitida" },
    };

    // Catálogos ficticios para los selectores (id, nombre)
    public static Dictionary<int, string> Clientes = new()
    {
        [1] = "Clínica San Felipe", [2] = "Hospital Rebagliati", [3] = "Clínica Anglo", [4] = "Luis Rojas (particular)"
    };
    public static Dictionary<int, string> Equipos = new()
    {
        [1] = "Artroscopio", [2] = "Sierra", [3] = "Taladro"
    };
    public static Dictionary<int, string> Bandejas = new()
    {
        [1] = "Bandeja Artroscopia", [2] = "Bandeja Cadera", [3] = "Bandeja Trauma", [4] = "Bandeja Columna"
    };
    public static string[] Estados = { "Programada", "En curso", "Culminada", "Cancelada" };

    // Texto oscuro sobre mostaza para que se lea bien
    public static string ColorTexto(string estado) => estado == "Programada" ? "#000000" : "#ffffff";

    public static string Color(string estado) => estado switch
    {
        "Programada" => "#E1AD01", // mostaza
        "En curso"   => "#198754", // verde
        "Culminada"  => "#6c757d", // gris
        "Cancelada"  => "#dc3545", // rojo
        _ => "#000000"
    };
}
