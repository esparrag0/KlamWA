using System.Net.Http.Json;
using KlamWA.Dtos;

namespace KlamWA.Services;

// GET clientes[?tipo=...]: clientes reales de MySQL para los selectores (reemplaza DatosClientes.Lista).
public class ClientesApiService : KlamApiBase
{
    public ClientesApiService(HttpClient http, SesionUsuario sesion) : base(http, sesion) { }

    public async Task<ResultadoApi<List<ClienteResumenDto>>> ListarAsync(string? tipoCliente = null)
    {
        try
        {
            var url = string.IsNullOrEmpty(tipoCliente) ? "clientes" : $"clientes?tipo={Uri.EscapeDataString(tipoCliente)}";
            return await LeerAsync<List<ClienteResumenDto>>(await Http.GetAsync(url));
        }
        catch (HttpRequestException) { return SinConexion<List<ClienteResumenDto>>(); }
    }

    // Alta de cliente con usuario y contraseña; la BL valida todo y devuelve su mensaje si falla.
    public async Task<ResultadoApi<ClienteResumenDto>> RegistrarAsync(ClienteRegistroDto dto)
    {
        try
        {
            return await LeerAsync<ClienteResumenDto>(await Http.PostAsJsonAsync("clientes", dto));
        }
        catch (HttpRequestException) { return SinConexion<ClienteResumenDto>(); }
    }

    // Detalle de un cliente (solo administrador). Los ids de clínica y paciente se solapan: siempre con tipo.
    public async Task<ResultadoApi<ClienteRegistroDto>> ObtenerAsync(string tipoCliente, int id)
    {
        try { return await LeerAsync<ClienteRegistroDto>(await Http.GetAsync($"clientes/{tipoCliente}/{id}")); }
        catch (HttpRequestException) { return SinConexion<ClienteRegistroDto>(); }
    }

    // Modifica los datos del cliente (usuario y contraseña no se cambian aquí).
    public async Task<ResultadoApi<bool>> ActualizarAsync(string tipoCliente, int id, ClienteRegistroDto dto)
    {
        try { return await LeerSinCuerpoAsync(await Http.PutAsJsonAsync($"clientes/{tipoCliente}/{id}", dto)); }
        catch (HttpRequestException) { return SinConexion<bool>(); }
    }
}
