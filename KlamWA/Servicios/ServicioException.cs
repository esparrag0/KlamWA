namespace KlamWA.Servicios;

/// Error al consumir el backend Java. El mensaje está listo para mostrarse al usuario:
/// si el servidor respondió {"error": "..."}, es ese texto; si no se pudo conectar, es un aviso genérico.
public class ServicioException : Exception
{
    public int? CodigoHttp { get; }

    public bool EsNoEncontrado => CodigoHttp == 404;
    public bool EsErrorDeValidacion => CodigoHttp == 400;
    public bool EsErrorDeConexion => CodigoHttp is null;

    public ServicioException(string mensaje, int? codigoHttp = null, Exception? causa = null)
        : base(mensaje, causa)
    {
        CodigoHttp = codigoHttp;
    }
}
