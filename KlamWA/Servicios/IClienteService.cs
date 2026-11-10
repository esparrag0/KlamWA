using KlamWA.ViewModels;

namespace KlamWA.Servicios;

public interface IClienteService
{
    Task<List<ClienteOpcionViewModel>> Listar(string? tipoCliente = null);
}
