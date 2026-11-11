using System.Net.Http.Json;
using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

/// <summary>
/// Cliente REST del historial de cirugias. No envia token: el backend aun no valida
/// sesion en este endpoint (RF01 pendiente de confirmacion con la profesora).
/// </summary>
public sealed class CirugiaConsultaService(HttpClient client) : ICirugiaConsultaService
{
    public async Task<List<CirugiaHistorialDTO>> HistorialClienteAsync(string tipoCliente, int idCliente)
    {
        var ruta = $"cirugias/historial?tipoCliente={Uri.EscapeDataString(tipoCliente)}&idCliente={idCliente}";
        using var respuesta = await client.GetAsync(ruta);
        await HttpUtils.VerificarRespuestaAsync(respuesta);
        return await respuesta.Content.ReadFromJsonAsync<List<CirugiaHistorialDTO>>() ?? [];
    }
}