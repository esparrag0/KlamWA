using System.Net.Http.Json;
using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

/// <summary>
/// Cliente REST del historial de cirugias. No envia token: el backend aun no valida
/// sesion en este endpoint (RF01 pendiente de confirmacion con la profesora).
/// </summary>
public sealed class CirugiaConsultaService(HttpClient client, AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : ICirugiaConsultaService
{
    public async Task<List<CirugiaHistorialDTO>> HistorialClienteAsync()
    {
        return await HttpUtilsAutenticado.DoGet<List<CirugiaHistorialDTO>>(
            client, "cirugias/historial", await tokens.ObtenerAsync(autenticacion)) ?? [];
    }
}
