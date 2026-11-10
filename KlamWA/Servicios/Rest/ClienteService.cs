using KlamWA.DTOs;
using KlamWA.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class ClienteService : IClienteService
{
    private readonly string _baseUrl;

    public ClienteService(IConfiguration configuracion)
    {
        _baseUrl = ServicioBase.Url(configuracion, "clientes");
    }

    public async Task<List<ClienteOpcionViewModel>> Listar(string? tipoCliente = null)
    {
        var url = string.IsNullOrWhiteSpace(tipoCliente)
            ? _baseUrl
            : QueryHelpers.AddQueryString(_baseUrl, "tipoCliente", tipoCliente.Trim());
        var dtos = await HttpUtils.DoGet<List<ClienteDTO>>(url);
        return dtos.Select(CirugiaMapeo.ToOpcion).ToList();
    }
}
