namespace KlamWA.Components.Shared;

// Solo datos ficticios para Lab 9 (sin BL, sin BD, sin servicios).
// Campos alineados con la clase Java Cirugia / Cotizacion.
// Estados en pantalla: Programada (PROGRAMADA), En curso (EN_PROCESO),
// Culminada (FINALIZADA), Cancelada (CANCELADA).
public class CirugiaMock
{
    public int Id { get; set; }
    public int? SolicitudId { get; set; }
    public DateTime Inicio { get; set; }
    public DateTime Fin { get; set; }
    public string Procedimiento { get; set; } = "";
    public string Doctor { get; set; } = "";
    public string Paciente { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public string Clinica { get; set; } = "";
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
    // Catálogo de muestra compartido por agenda, historial y reportes. Ninguna página lo modifica.
    public static IReadOnlyList<CirugiaMock> Cirugias { get; } = Array.AsReadOnly<CirugiaMock>([
        new() { Id=1, SolicitudId=1, Inicio=new DateTime(2026,10,5,8,0,0),   Fin=new DateTime(2026,10,5,10,0,0),  Procedimiento="Artroscopia de rodilla", Doctor="Dr. Ramos", Paciente="Ana Medina", Diagnostico="Lesión de menisco medial", Clinica="Clínica San Felipe", Cliente="Clínica San Felipe",   Equipo="Artroscopio", Bandeja="Bandeja Artroscopia", Estado="Culminada", PrecioCotizacion=4500, EstadoCotizacion="Aceptada" },
        new() { Id=2, Inicio=new DateTime(2026,10,12,10,30,0),Fin=new DateTime(2026,10,12,13,0,0), Procedimiento="Reemplazo de cadera",    Doctor="Dra. Lima", Paciente="Rosa Paredes", Diagnostico="Artrosis de cadera", Clinica="Hospital Rebagliati", Cliente="Hospital Rebagliati", Equipo="Sierra",      Bandeja="Bandeja Cadera",      Estado="Cancelada", MotivoCancelacion="Paciente no apto", PrecioCotizacion=8200, EstadoCotizacion="Rechazada" },
        new() { Id=3, Inicio=new DateTime(2026,10,26,9,0,0),  Fin=new DateTime(2026,10,26,12,0,0), Procedimiento="Osteosíntesis de tibia", Doctor="Dr. Ramos", Paciente="Luis Cárdenas", Diagnostico="Fractura de tibia", Clinica="Clínica San Felipe", Cliente="Clínica San Felipe",   Equipo="Taladro",     Bandeja="Bandeja Trauma",      Estado="En curso",  PrecioCotizacion=6000, EstadoCotizacion="Aceptada" },
        new() { Id=4, SolicitudId=2, Inicio=new DateTime(2026,10,28,14,0,0), Fin=new DateTime(2026,10,28,16,0,0), Procedimiento="Artroscopia de hombro", Doctor="Dr. Vega", Paciente="Luis Rojas", Diagnostico="Lesión de hombro", Clinica="Clínica Anglo", Cliente="Luis Rojas (particular)", Equipo="Artroscopio", Bandeja="Bandeja Artroscopia", Estado="Programada", PrecioCotizacion=4800, EstadoCotizacion="Emitida" },
        new() { Id=5, SolicitudId=3, Inicio=new DateTime(2026,10,30,11,0,0), Fin=new DateTime(2026,10,30,15,0,0), Procedimiento="Cirugía de columna",    Doctor="Dra. Lima", Paciente="María Vega", Diagnostico="Hernia discal L4-L5", Clinica="Clínica Anglo", Cliente="Clínica Anglo",       Equipo="Taladro",     Bandeja="Bandeja Columna",     Estado="Programada", PrecioCotizacion=12000, EstadoCotizacion="Emitida" },
        new() { Id=6, Inicio=new DateTime(2026,10,20,8,0,0), Fin=new DateTime(2026,10,20,11,0,0), Procedimiento="Cirugía de columna", Doctor="Dra. Lima", Paciente="Carlos Ruiz", Diagnostico="Hernia discal", Clinica="Clínica San Felipe", Cliente="Clínica San Felipe", Equipo="Taladro", Bandeja="Bandeja Columna", Estado="Culminada", PrecioCotizacion=11000, EstadoCotizacion="Aceptada" },
        new() { Id=7, Inicio=new DateTime(2026,10,22,9,0,0), Fin=new DateTime(2026,10,22,12,0,0), Procedimiento="Craneotomía", Doctor="Dr. Salazar", Paciente="Elena Gómez", Diagnostico="Lesión intracraneal", Clinica="Clínica San Felipe", Cliente="Clínica San Felipe", Equipo="Taladro", Bandeja="Bandeja Cráneo", Estado="Culminada", PrecioCotizacion=13500, EstadoCotizacion="Aceptada" },
        new() { Id=8, Inicio=new DateTime(2026,10,23,8,0,0), Fin=new DateTime(2026,10,23,12,0,0), Procedimiento="Cirugía de columna", Doctor="Dra. Lima", Paciente="Jorge Peña", Diagnostico="Hernia discal", Clinica="Clínica Anglo", Cliente="Clínica Anglo", Equipo="Taladro", Bandeja="Bandeja Columna", Estado="Culminada", PrecioCotizacion=11800, EstadoCotizacion="Aceptada" },
    ]);

    // Selección fija de muestras por perfil; sin consultas ni autorización real.
    public static IEnumerable<CirugiaMock> ParaPerfil(string perfil) => perfil switch
    {
        "instrumentista" => Cirugias.Where(c => c.Id is 1 or 3 or 4 or 5 or 6 or 7 or 8),
        "vendedor" => Cirugias.Where(c => c.Id is 2 or 4),
        "cliente" => Cirugias.Where(c => c.Cliente == "Clínica San Felipe"),
        _ => Cirugias
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
        [1] = "Bandeja Artroscopia", [2] = "Bandeja Cadera", [3] = "Bandeja Trauma", [4] = "Bandeja Columna", [5] = "Bandeja Cráneo"
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
