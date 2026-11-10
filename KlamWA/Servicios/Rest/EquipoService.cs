using KlamWA.DTOs;
using KlamWA.ViewModels;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class EquipoService : IEquipoService
{
    private readonly string _baseUrl;

    public EquipoService(IConfiguration configuracion)
    {
        _baseUrl = ServicioBase.Url(configuracion, "equipos");
    }

    public async Task<List<EquipoOpcionViewModel>> Listar(bool soloDisponibles = false, string? categoria = null)
    {
        var parametros = new Dictionary<string, string?>();
        if (soloDisponibles)
        {
            parametros["soloDisponibles"] = "true";
        }
        if (!string.IsNullOrWhiteSpace(categoria))
        {
            parametros["categoria"] = categoria.Trim();
        }

        var dtos = await HttpUtils.DoGet<List<EquipoDTO>>(QueryHelpers.AddQueryString(_baseUrl, parametros));
        return dtos.Select(CirugiaMapeo.ToOpcion).ToList();
    }
}
