using System.Net.Http.Json;
using KlamWA.Dtos;

namespace KlamWA.Services;

// RF05. Pantalla C# -> HttpClient -> REST Java (ClientesSolicitudesResource) -> BL -> DAO -> MySQL.
public class SolicitudesApiService : KlamApiBase
{
    public SolicitudesApiService(HttpClient http, SesionUsuario sesion) : base(http, sesion) { }

    public async Task<ResultadoApi<SolicitudCirugiaDto>> RegistrarAsync(
        string tipoCliente, int idCliente, SolicitudCirugiaRequestDto dto)
    {
        try
        {
            var r = await Http.PostAsJsonAsync($"clientes/{tipoCliente}/{idCliente}/solicitudes", dto);
            return await LeerAsync<SolicitudCirugiaDto>(r);
        }
        catch (HttpRequestException) { return SinConexion<SolicitudCirugiaDto>(); }
    }

    public async Task<ResultadoApi<List<SolicitudCirugiaDto>>> ListarPorClienteAsync(string tipoCliente, int idCliente)
    {
        try
        {
            var r = await Http.GetAsync($"clientes/{tipoCliente}/{idCliente}/solicitudes");
            return await LeerAsync<List<SolicitudCirugiaDto>>(r);
        }
        catch (HttpRequestException) { return SinConexion<List<SolicitudCirugiaDto>>(); }
    }
}
