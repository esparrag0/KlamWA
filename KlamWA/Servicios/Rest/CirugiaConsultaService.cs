using System.Net.Http.Json;
using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

/// <summary>Cliente REST del historial de cirugías (RF10), con el token de la sesión.</summary>
public sealed class CirugiaConsultaService(HttpClient client, AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : ICirugiaConsultaService
{
    public async Task<List<CirugiaHistorialDTO>> HistorialClienteAsync()
    {
        return await HttpUtilsAutenticado.DoGet<List<CirugiaHistorialDTO>>(
            client, "cirugias/historial", await tokens.ObtenerAsync(autenticacion)) ?? [];
    }

    public async Task<List<CirugiaHistorialDTO>> HistorialDeClienteAsync(string tipoCliente, int idCliente)
    {
        try
        {
            return await HttpUtilsAutenticado.DoGet<List<CirugiaHistorialDTO>>(client,
                $"cirugias/historial?tipoCliente={Uri.EscapeDataString(tipoCliente)}&idCliente={idCliente}",
                await tokens.ObtenerAsync(autenticacion)) ?? [];
        }
        catch (HttpRequestException ex)
        {
            throw new ServicioException(ex.Message, (int?)ex.StatusCode, ex);
        }
    }
}
