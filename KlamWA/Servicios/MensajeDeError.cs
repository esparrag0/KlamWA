using System.Net;

namespace KlamWA.Servicios;

/// <summary>Tono de un aviso en pantalla. Cuatro, y el criterio es quién puede arreglarlo.</summary>
public enum TipoAviso
{
    /// <summary>Contexto, no es un problema. Teal.</summary>
    Info,
    /// <summary>La acción se completó. Verde.</summary>
    Exito,
    /// <summary>No se pudo, y está en manos de quien lo usa: un dato, un estado, un permiso. Ámbar.</summary>
    Atencion,
    /// <summary>Falló el sistema. No es culpa de quien lo usa y lo que toca es reintentar. Rosa.</summary>
    Error
}

/// <summary>
/// Traductor único de errores técnicos a lenguaje de la plataforma.
///
/// Por qué existe: las pantallas muestran <c>ex.Message</c> en más de cien sitios. Si el
/// mensaje nace humano, todas lo muestran bien sin tocarlas; si nace técnico, el cliente
/// acaba leyendo "The request was canceled due to the configured HttpClient.Timeout of
/// 100 seconds elapsing". Así que se traduce donde se construye la excepción
/// (HttpUtils y HttpUtilsAutenticado) y no en cada pantalla.
///
/// Regla de redacción: decir qué pasó y qué hacer, en el vocabulario de Klam
/// (cirugías, solicitudes, documentos, servidor). Nunca códigos, clases ni trazas.
/// </summary>
public static class MensajeDeError
{
    // Señales de que un texto viene de .NET y no de la lógica de negocio. Si aparece
    // cualquiera, el mensaje no se enseña: se cambia por el genérico que corresponda.
    private static readonly string[] Delatores =
    [
        "httpclient", "httprequest", "socketexception", "ioexception", "taskcanceled",
        "system.", "stack trace", "   at ", "inner exception", "object reference",
        "status code", "statuscode", "no connection could be made", "unable to connect",
        "actively refused", "the ssl connection", "elapsing", "timeout of",
        "does not indicate success", "connection refused", "name or service not known",
        "an error occurred while sending the request", "unexpected character",
        "json", "deserializ", "null"
    ];

    private const string SinConexion =
        "No se pudo conectar con el servidor de Klam. Comprueba que el servicio esté encendido y vuelve a intentarlo.";
    private const string Tardo =
        "El servidor tardó demasiado en responder. Vuelve a intentarlo en un momento.";
    private const string FalloServidor =
        "El servidor tuvo un problema al procesar la solicitud. Vuelve a intentarlo; si sigue igual, avisa al equipo.";
    private const string Inesperado =
        "Ocurrió un problema inesperado. Vuelve a intentarlo.";

    /// <summary>
    /// Decide si un texto puede enseñarse tal cual. El mensaje que manda el backend
    /// ({"error": "..."}) sí es apto; el que fabrica .NET, no.
    /// </summary>
    public static bool EsPresentable(string? mensaje)
    {
        if (string.IsNullOrWhiteSpace(mensaje)) return false;
        var m = mensaje.ToLowerInvariant();
        if (Delatores.Any(d => m.Contains(d))) return false;
        // Un párrafo largo tampoco se enseña: suele ser una traza disfrazada.
        return mensaje.Length <= 240;
    }

    /// <summary>Mensaje para un código de estado cuando el backend no mandó uno propio.</summary>
    public static (string Texto, TipoAviso Tipo) PorCodigo(int codigo) => codigo switch
    {
        400 => ("Revisa los datos del formulario: el servidor no los aceptó.", TipoAviso.Atencion),
        401 => ("Tu sesión expiró. Vuelve a iniciar sesión para continuar.", TipoAviso.Atencion),
        403 => ("Tu perfil no tiene permiso para esta acción.", TipoAviso.Atencion),
        404 => ("No encontramos ese registro. Puede que lo hayan eliminado o que el enlace esté desactualizado.", TipoAviso.Atencion),
        409 => ("No se puede hacer esto con el estado actual del registro.", TipoAviso.Atencion),
        413 => ("El archivo pesa más de lo permitido.", TipoAviso.Atencion),
        >= 500 => (FalloServidor, TipoAviso.Error),
        _ => (Inesperado, TipoAviso.Error)
    };

    /// <summary>
    /// Construye el texto definitivo: usa el del backend si es presentable y, si no,
    /// el que corresponda al código. Lo llaman HttpUtils y HttpUtilsAutenticado al
    /// levantar la excepción, de modo que lo que viaja hacia las pantallas ya es legible.
    /// </summary>
    public static string Humanizar(string? mensajeDelServidor, int? codigoHttp)
    {
        if (EsPresentable(mensajeDelServidor)) return mensajeDelServidor!.Trim();
        return codigoHttp is null ? SinConexion : PorCodigo(codigoHttp.Value).Texto;
    }

    /// <summary>Traduce cualquier excepción a texto y tono. Para los catch genéricos.</summary>
    public static (string Texto, TipoAviso Tipo) De(Exception? ex)
    {
        switch (ex)
        {
            case null:
                return (Inesperado, TipoAviso.Error);

            // El tiempo de espera de HttpClient llega como TaskCanceled, no como Timeout.
            case TaskCanceledException:
            case OperationCanceledException:
            case TimeoutException:
                return (Tardo, TipoAviso.Error);

            case ServicioException servicio:
                if (servicio.CodigoHttp is int c) return Mezclar(servicio.Message, c);
                return EsPresentable(servicio.Message)
                    ? (servicio.Message, TipoAviso.Atencion)
                    : (SinConexion, TipoAviso.Error);

            case HttpRequestException http:
                if (http.StatusCode is HttpStatusCode estado) return Mezclar(http.Message, (int)estado);
                return (SinConexion, TipoAviso.Error);

            // Falta RestApi:BaseUrl en appsettings.json y cosas por el estilo.
            case InvalidOperationException:
                return ("La aplicación no tiene configurada la dirección del servidor. Avisa al equipo.", TipoAviso.Error);

            case IOException:
                return ("No se pudo leer el archivo. Vuelve a seleccionarlo.", TipoAviso.Atencion);

            default:
                return EsPresentable(ex.Message) ? (ex.Message, TipoAviso.Atencion) : (Inesperado, TipoAviso.Error);
        }
    }

    /// <summary>Solo el texto, para los sitios que únicamente pintan una cadena.</summary>
    public static string Texto(Exception? ex) => De(ex).Texto;

    /// <summary>Solo el tono, para elegir el color del aviso.</summary>
    public static TipoAviso Tono(Exception? ex) => De(ex).Tipo;

    // El texto del backend manda si es presentable; el tono siempre lo decide el código,
    // porque es lo que dice de quién depende arreglarlo.
    private static (string, TipoAviso) Mezclar(string? mensaje, int codigo)
    {
        var (porDefecto, tono) = PorCodigo(codigo);
        return (EsPresentable(mensaje) ? mensaje!.Trim() : porDefecto, tono);
    }
}
