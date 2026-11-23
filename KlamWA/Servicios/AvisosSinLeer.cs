using KlamWA.DTOs;

namespace KlamWA.Servicios;

/// Contador de avisos sin leer de la sesión (RF12). Lo comparten el menú, la campana y el inicio,
/// y la bandeja lo actualiza al marcar avisos como leídos. Si falla la consulta queda en null y no se muestra.
/// Además detecta avisos nuevos durante el sondeo y los publica en <see cref="Nuevo"/> para mostrarlos como emergente.
public sealed class AvisosSinLeer(INotificacionService notificaciones)
{
    private int? totalConocido;
    private int ultimoIdVisto;

    public int? Cantidad { get; private set; }
    public event Action? Cambio;
    public event Action<NotificacionDTO>? Nuevo;
    /// El backend respondió 401: la sesión ya no existe allí (p. ej. se reinició el servidor).
    public event Action? SesionExpirada;
    private readonly List<NotificacionDTO> pendientes = [];

    /// Avisos detectados cuando aún no había nadie escuchando (p. ej. al iniciar sesión); quien se suscribe los recoge.
    public List<NotificacionDTO> TomarPendientes()
    {
        var copia = pendientes.ToList();
        pendientes.Clear();
        return copia;
    }

    private void Publicar(NotificacionDTO aviso)
    {
        if (Nuevo is null) pendientes.Add(aviso); else Nuevo(aviso);
    }

    public async Task ActualizarAsync()
    {
        try
        {
            var resumen = await notificaciones.ResumenAsync();
            Cantidad = resumen.NoLeidas;
            await DetectarNuevosAsync(resumen.Total);
        }
        catch (ServicioException ex) when (ex.CodigoHttp == 401)
        {
            Cantidad = null;
            SesionExpirada?.Invoke();
        }
        catch (Exception)
        {
            // Sin sesión, sin permiso o sin backend: el contador simplemente no se muestra.
            Cantidad = null;
        }
        Cambio?.Invoke();
    }

    private async Task DetectarNuevosAsync(int total)
    {
        var primeraVez = totalConocido is null;
        var hayMas = totalConocido is not null && total > totalConocido;
        totalConocido = total;
        if (!primeraVez && !hayMas) return;

        var lista = await notificaciones.ListarAsync();
        if (primeraVez)
        {
            // Al entrar: los avisos sin leer que ya existían también salen como emergente (los 3 más recientes).
            ultimoIdVisto = lista.Count == 0 ? 0 : lista.Max(n => n.Id);
            foreach (var aviso in lista.Where(n => !n.Leida).OrderBy(n => n.Id).TakeLast(3)) Publicar(aviso);
            return;
        }
        var nuevos = lista.Where(n => n.Id > ultimoIdVisto && !n.Leida).OrderBy(n => n.Id).ToList();
        if (lista.Count > 0) ultimoIdVisto = Math.Max(ultimoIdVisto, lista.Max(n => n.Id));
        foreach (var aviso in nuevos.TakeLast(3)) Publicar(aviso);
    }
}
