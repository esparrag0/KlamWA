namespace KlamWA.Components.Shared;

// Solo datos ficticios para Lab 9 (sin BL, sin BD, sin servicios).
// Campos alineados con las clases Java Cliente / PacienteParticular / ClinicaHospital
// y con DocumentoIngreso (TipoDocumentoIngreso: DNI, RECETA_MEDICA, ORDEN_COMPRA;
// EstadoValidacionDocumento: PENDIENTE, VALIDADO, RECHAZADO).
// Los DNI y RUC de abajo son inventados.
public class ClienteMock
{
    public int Id { get; set; }
    public string Tipo { get; set; } = DatosClientes.Particular;
    public string Nombre { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Email { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Dni { get; set; } = "";               // solo particular
    public bool PagoConfirmado { get; set; }            // solo particular
    public string Ruc { get; set; } = "";               // solo clínica/hospital
    public bool TieneConsignacion { get; set; }         // solo clínica/hospital
    public string PeriodoCredito { get; set; } = "";    // solo clínica/hospital
    public bool Activo { get; set; } = true;

    public string Documento => Tipo == DatosClientes.Particular ? "DNI " + Dni : "RUC " + Ruc;
}

public class DocumentoMock
{
    public string Tipo { get; set; } = "";      // DNI, Receta médica, Orden de compra
    public string Archivo { get; set; } = "";
    public string Estado { get; set; } = "";    // Pendiente, Validado, Rechazado
}

public class SolicitudMock
{
    public int Id { get; set; }
    public int IdCliente { get; set; }
    public string Cliente { get; set; } = "";
    public string Clinica { get; set; } = "";          // clínica u hospital donde se realizará
    public string Procedimiento { get; set; } = "";
    public string Doctor { get; set; } = "";
    public string Diagnostico { get; set; } = "";
    public DateTime Fecha { get; set; }
    public string Estado { get; set; } = "";    // Por revisar, Documentación incompleta, Rechazada · Resubida habilitada, Aprobada (mismos nombres que usa el Rol 4)
    public string MotivoRechazo { get; set; } = ""; // mensaje del administrador (RF07)
    public List<DocumentoMock> Documentos { get; set; } = new();
}

public static class DatosClientes
{
    public const string Particular = "Particular";
    public const string Clinica = "Clínica/Hospital";

    // El comprobante de pago bancario aplica a clientes particulares (RF08).
    public const string ComprobantePago = "Comprobante de pago bancario";
    public static string[] TiposDocumento = { "DNI", "Receta médica", "Orden de compra", ComprobantePago };

    public static List<ClienteMock> Lista = new()
    {
        new() { Id=1, Tipo=Clinica,    Nombre="Clínica San Felipe",     Direccion="Av. Gregorio Escobedo 650, Jesús María", Email="compras@sanfelipe.example", Telefono="014710000", Ruc="20100000011", TieneConsignacion=true,  PeriodoCredito="30 días" },
        new() { Id=2, Tipo=Clinica,    Nombre="Hospital Rebagliati",    Direccion="Av. Rebagliati 490, Jesús María",        Email="logistica@rebagliati.example", Telefono="012651000", Ruc="20100000022", TieneConsignacion=false, PeriodoCredito="60 días" },
        new() { Id=3, Tipo=Clinica,    Nombre="Clínica Anglo",          Direccion="Av. Alfredo Salazar 350, San Isidro",    Email="contacto@anglo.example", Telefono="012219000", Ruc="20100000033", TieneConsignacion=true,  PeriodoCredito="15 días" },
        new() { Id=4, Tipo=Particular, Nombre="Luis Rojas (particular)", Direccion="Calle Los Pinos 123, Miraflores",       Email="luis.rojas@example.com", Telefono="987654321", Dni="40000001", PagoConfirmado=true },
        new() { Id=5, Tipo=Particular, Nombre="María Quispe (particular)", Direccion="Jr. Las Flores 45, Surco",            Email="maria.quispe@example.com", Telefono="912345678", Dni="40000002", PagoConfirmado=false },
        new() { Id=6, Tipo=Clinica,    Nombre="Clínica Santa Teresa",   Direccion="Av. Primavera 1200, Surco",              Email="info@santateresa.example", Telefono="013450000", Ruc="20100000044", TieneConsignacion=false, PeriodoCredito="30 días", Activo=false },
    };

    // Clínicas y hospitales activos (para elegir dónde se realizará la cirugía)
    public static Dictionary<int, string> Clinicas =>
        Lista.Where(c => c.Activo && c.Tipo == Clinica).ToDictionary(c => c.Id, c => c.Nombre);

    public static List<SolicitudMock> Solicitudes = new()
    {
        new() { Id=1, IdCliente=1, Cliente="Clínica San Felipe", Clinica="Clínica San Felipe", Procedimiento="Artroscopia de rodilla", Doctor="Dr. Ramos",
                Diagnostico="Lesión de menisco medial", Fecha=new DateTime(2026,11,4,8,0,0), Estado="Aprobada",
                Documentos = new() {
                    new() { Tipo="Orden de compra", Archivo="oc-0001.pdf", Estado="Validado" },
                    new() { Tipo="Receta médica",   Archivo="receta-0001.pdf", Estado="Validado" } } },
        new() { Id=2, IdCliente=4, Cliente="Luis Rojas (particular)", Clinica="Clínica Anglo", Procedimiento="Artroscopia de hombro", Doctor="Dr. Vega",
                Diagnostico="Desgarro del manguito rotador", Fecha=new DateTime(2026,11,10,14,0,0), Estado="Por revisar",
                Documentos = new() {
                    new() { Tipo="DNI",           Archivo="dni-luis.jpg", Estado="Validado" },
                    new() { Tipo="Receta médica", Archivo="receta-luis.pdf", Estado="Pendiente" },
                    new() { Tipo=ComprobantePago, Archivo="pago-luis.pdf", Estado="Pendiente" } } },
        new() { Id=3, IdCliente=3, Cliente="Clínica Anglo", Clinica="Clínica Anglo", Procedimiento="Cirugía de columna", Doctor="Dra. Lima",
                Diagnostico="Hernia discal L4-L5", Fecha=new DateTime(2026,11,18,11,0,0), Estado="Rechazada · Resubida habilitada",
                MotivoRechazo="La orden de compra no se puede leer. Suba una copia más nítida.",
                Documentos = new() {
                    new() { Tipo="Orden de compra", Archivo="oc-0003.pdf", Estado="Rechazado" } } },
    };
}
