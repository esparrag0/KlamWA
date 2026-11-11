namespace KlamWA.Servicios;

/// Contador de avisos sin leer de la sesión (RF12). Lo comparten el menú, la campana y el inicio,
/// y la bandeja lo actualiza al marcar avisos como leídos. Si falla la consulta queda en null y no se muestra.
public sealed class AvisosSinLeer(INotificacionService notificaciones)
{
    public int? Cantidad { get; private set; }
    public event Action? Cambio;

    public async Task ActualizarAsync()
    {
        try
        {
            Cantidad = (await notificaciones.ResumenAsync()).NoLeidas;
        }
        catch (Exception)
        {
            // Sin sesión, sin permiso o sin backend: el contador simplemente no se muestra.
            Cantidad = null;
        }
        Cambio?.Invoke();
    }
}
