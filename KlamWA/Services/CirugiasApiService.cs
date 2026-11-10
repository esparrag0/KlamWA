using System.Net.Http.Json;
using KlamWA.Dtos;

namespace KlamWA.Services;

// RF09. Integrante 5 (RF02) agrega aquí listar/modificar/eliminar sobre el mismo recurso "cirugias".
public class CirugiasApiService : KlamApiBase
{
    public CirugiasApiService(HttpClient http, SesionUsuario sesion) : base(http, sesion) { }

    public async Task<ResultadoApi<CirugiaRegistradaDto>> RegistrarAsync(CirugiaRequestDto dto)
    {
        try
        {
            var r = await Http.PostAsJsonAsync("cirugias", dto);
            return await LeerAsync<CirugiaRegistradaDto>(r);
        }
        catch (HttpRequestException) { return SinConexion<CirugiaRegistradaDto>(); }
    }
}
