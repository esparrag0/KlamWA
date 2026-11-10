using System.Net;
using System.Net.Http.Json;
using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

// Consume CirugiasResource (RF02) del backend Java
public class CirugiaRestService : ICirugiaService
{
    private readonly HttpClient http;

    public CirugiaRestService(HttpClient http)
    {
        this.http = http;
    }

    public async Task<CirugiaDTO?> ObtenerAsync(int id)
    {
        using HttpResponseMessage respuesta = await http.GetAsync($"cirugias/{id}");
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        return await RespuestaRest.LeerAsync<CirugiaDTO>(respuesta);
    }

    public async Task<OpcionesCirugiaDTO> OpcionesAsync()
    {
        using HttpResponseMessage respuesta = await http.GetAsync("cirugias/opciones");
        return await RespuestaRest.LeerAsync<OpcionesCirugiaDTO>(respuesta);
    }

    public async Task<CirugiaDTO> RegistrarAsync(CirugiaFormDTO cirugia)
    {
        using HttpResponseMessage respuesta = await http.PostAsJsonAsync("cirugias", cirugia);
        return await RespuestaRest.LeerAsync<CirugiaDTO>(respuesta);
    }

    public async Task<CirugiaDTO> ModificarAsync(int id, CirugiaFormDTO cirugia)
    {
        using HttpResponseMessage respuesta = await http.PutAsJsonAsync($"cirugias/{id}", cirugia);
        return await RespuestaRest.LeerAsync<CirugiaDTO>(respuesta);
    }

    public async Task<CirugiaDTO> CambiarEstadoAsync(int id, string estado)
    {
        using HttpResponseMessage respuesta = await http.PostAsJsonAsync($"cirugias/{id}/estado",
            new AccionCirugiaDTO { Estado = estado });
        return await RespuestaRest.LeerAsync<CirugiaDTO>(respuesta);
    }

    public async Task<CirugiaDTO> CancelarAsync(int id, string motivo)
    {
        using HttpResponseMessage respuesta = await http.PostAsJsonAsync($"cirugias/{id}/cancelacion",
            new AccionCirugiaDTO { Motivo = motivo });
        return await RespuestaRest.LeerAsync<CirugiaDTO>(respuesta);
    }

    public async Task EliminarAsync(int id)
    {
        using HttpResponseMessage respuesta = await http.DeleteAsync($"cirugias/{id}");
        await RespuestaRest.VerificarAsync(respuesta);
    }
}
