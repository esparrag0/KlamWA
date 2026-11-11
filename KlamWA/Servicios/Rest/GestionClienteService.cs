using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

public class GestionClienteService : IGestionClienteService
{
    private string BaseUrl { get; set; }
    private string BaseUrlClientes { get; set; }

    public GestionClienteService(IConfiguration configuracion)
    {
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/gestion-clientes";
        BaseUrlClientes = $"{configuracion["RestApi:BaseUrl"]!}/clientes";
    }

    public async Task<List<ClienteGestionDTO>> ListarAsync(string? tipoCliente = null)
    {
        string url = string.IsNullOrWhiteSpace(tipoCliente)
            ? BaseUrl
            : $"{BaseUrl}?tipo={Uri.EscapeDataString(tipoCliente)}";
        return await HttpUtils.DoGet<List<ClienteGestionDTO>>(url) ?? [];
    }

    public async Task<ClienteGestionDTO> RegistrarAsync(ClienteFormDTO cliente) =>
        await HttpUtils.DoPost<ClienteFormDTO, ClienteGestionDTO>(BaseUrl, cliente);

    public async Task<ClienteFormDTO> ObtenerAsync(string tipoCliente, int id) =>
        await HttpUtils.DoGet<ClienteFormDTO>($"{BaseUrl}/{Uri.EscapeDataString(tipoCliente)}/{id}");

    public async Task ActualizarAsync(string tipoCliente, int id, ClienteFormDTO cliente) =>
        await HttpUtils.DoPut($"{BaseUrl}/{Uri.EscapeDataString(tipoCliente)}/{id}", cliente);

    public async Task HabilitarCuentaAsync(string tipoCliente, int id, string username, string password) =>
        await HttpUtils.DoPost<object, System.Text.Json.JsonElement>(
            $"{BaseUrlClientes}/{Uri.EscapeDataString(tipoCliente)}/{id}/cuenta",
            new { userName = username, password });
}
