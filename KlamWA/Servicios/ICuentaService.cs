namespace KlamWA.Servicios;

// Cuenta del usuario con sesión (cualquier rol).
public interface ICuentaService
{
    Task CambiarPasswordAsync(string passwordActual, string passwordNueva);
}
