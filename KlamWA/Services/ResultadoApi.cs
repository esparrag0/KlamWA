namespace KlamWA.Services;

// Resultado de una llamada al REST: datos si salió bien, o el mensaje real del servidor (BL) si no.
public sealed class ResultadoApi<T>
{
    public bool Ok { get; private init; }
    public T? Datos { get; private init; }
    public string? Error { get; private init; }

    public static ResultadoApi<T> Exito(T datos) => new() { Ok = true, Datos = datos };
    public static ResultadoApi<T> Falla(string error) => new() { Ok = false, Error = error };
}
