namespace KlamWA.Servicios;

// Error devuelto por el backend (mensaje del campo "error" de la respuesta REST)
public class ServicioException : Exception
{
    public ServicioException(string mensaje) : base(mensaje)
    {
    }

    public ServicioException(string mensaje, Exception causa) : base(mensaje, causa)
    {
    }
}
