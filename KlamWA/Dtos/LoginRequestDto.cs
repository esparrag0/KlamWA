namespace KlamWA.Dtos;

// POST login (Java AuthResource lee "userName" y "password").
public class LoginRequestDto
{
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
}
