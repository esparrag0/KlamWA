namespace KlamWA.Dtos;

// Respuesta de POST login: id del usuario (o del cliente), su username y su rol
// (ADMINISTRADOR | VENDEDOR | TECNICO_INSTRUMENTISTA | CLINICA_HOSPITAL | PACIENTE_PARTICULAR).
public class UsuarioSesionDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string Rol { get; set; } = "";
    public string Token { get; set; } = "";   // "Authorization: Bearer ..." en las llamadas protegidas
}
