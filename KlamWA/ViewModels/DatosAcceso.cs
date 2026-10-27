namespace KlamWA.ViewModels;

// Datos inmutables del Lab. 9, basados en UsuarioPlataforma y Notificacion de Java.
// Son modelos de presentación, no entidades persistentes ni servicios de negocio.
public sealed record UsuarioVista(int Id, string Nombres, string Apellidos, string Username,
    string Email, string Rol, bool Activo, string? Especialidad = null)
{
    public string NombreCompleto => $"{Nombres} {Apellidos}";
    public string Iniciales => $"{Nombres[0]}{Apellidos[0]}";
}

public sealed record NotificacionVista(int Id, string Perfil, string Titulo, string Mensaje,
    string Fecha, bool Leida, string Categoria, string Destinatario, string? Enlace = null);

public sealed record PerfilVista(string Clave, string Nombre, string Persona, string Iniciales,
    string Descripcion);

public static class DatosAcceso
{
    public static IReadOnlyList<PerfilVista> Perfiles { get; } = Array.AsReadOnly<PerfilVista>([
        new("administrador", "Administrador", "Valeria Torres", "VT", "Supervisa la coordinación quirúrgica y administra los accesos del equipo."),
        new("vendedor", "Vendedor", "Diego Mendoza", "DM", "Registra solicitudes con documentos y consulta las cirugías que coordinas."),
        new("instrumentista", "Instrumentista", "Camila Rojas", "CR", "Consulta el calendario, las cirugías en las que participas y tus notificaciones."),
        new("cliente", "Cliente", "Clínica San Felipe", "SF", "Solicita una cirugía, adjunta documentos y revisa tus cotizaciones.")
    ]);

    public static PerfilVista? Perfil(string? clave) => Perfiles.FirstOrDefault(p => p.Clave == clave);
    public static IReadOnlyList<UsuarioVista> Usuarios { get; } = Array.AsReadOnly<UsuarioVista>([
        new(1, "Valeria", "Torres", "vtorres", "valeria.torres@bioklam.example", "Administrador", true),
        new(2, "Diego", "Mendoza", "dmendoza", "diego.mendoza@bioklam.example", "Vendedor", true),
        new(3, "Camila", "Rojas", "crojas", "camila.rojas@bioklam.example", "Instrumentista", true, "Cirugía de columna"),
        new(4, "Lucía", "Salazar", "lsalazar", "lucia.salazar@bioklam.example", "Administrador", true),
        new(5, "Andrés", "Vega", "avega", "andres.vega@bioklam.example", "Instrumentista", true, "Cirugía de cráneo"),
        new(6, "Mariana", "Castro", "mcastro", "mariana.castro@bioklam.example", "Vendedor", false)
    ]);
    public static UsuarioVista? Usuario(int id) => Usuarios.FirstOrDefault(u => u.Id == id);

    // Bandejas de ejemplo separadas: la selección solo navega entre maquetas.
    public static IReadOnlyList<NotificacionVista> AvisosAdministrador { get; } = Array.AsReadOnly<NotificacionVista>([
        new(101, "administrador", "Nueva solicitud de cirugía", "Clínica San Felipe envió una solicitud de cirugía de columna. La documentación está pendiente de revisión administrativa.", "26 oct. 2026 · 10:35", false, "Solicitud", "Valeria Torres"),
        new(102, "administrador", "Nueva solicitud de cirugía", "Se recibió una solicitud de Clínica Anglo para un procedimiento de cráneo. Revisa los datos y documentos antes de su aprobación.", "26 oct. 2026 · 09:20", false, "Solicitud", "Valeria Torres"),
        new(103, "administrador", "Nueva solicitud de cirugía", "El equipo comercial registró la solicitud del cliente particular Luis Rojas. El aviso aparece también en el canal de correo ilustrativo.", "25 oct. 2026 · 16:45", true, "Solicitud", "Valeria Torres")
    ]);
    public static IReadOnlyList<NotificacionVista> AvisosInstrumentista { get; } = Array.AsReadOnly<NotificacionVista>([
        new(201, "instrumentista", "Nueva cirugía en el calendario", "Se programó una cirugía de columna en Clínica Anglo para el 30 de octubre a las 11:00. Consulta el detalle de la programación.", "26 oct. 2026 · 10:15", false, "Programación", "Camila Rojas", "/agenda/cirugia/5"),
        new(202, "instrumentista", "Nueva cirugía en el calendario", "Se registró una artroscopia de hombro para el 28 de octubre a las 14:00, asociada al cliente Luis Rojas.", "25 oct. 2026 · 15:30", false, "Programación", "Camila Rojas", "/agenda/cirugia/4"),
        new(203, "instrumentista", "Nueva cirugía en el calendario", "La osteosíntesis de tibia de Clínica San Felipe ya está en la agenda del 26 de octubre, a las 09:00.", "24 oct. 2026 · 11:00", true, "Programación", "Camila Rojas", "/agenda/cirugia/3")
    ]);
    public static NotificacionVista? Aviso(int id) => AvisosAdministrador.Concat(AvisosInstrumentista).FirstOrDefault(n => n.Id == id);
}

// Estado de presentación por circuito, sin cookies, tokens ni almacenamiento.
public sealed class VistaPreviaRol
{
    public PerfilVista Perfil { get; private set; } = DatosAcceso.Perfiles[0];
    public event Action? Cambio;
    public void Mostrar(PerfilVista perfil)
    {
        if (Perfil.Clave == perfil.Clave) return;
        Perfil = perfil;
        Cambio?.Invoke();
    }
}
