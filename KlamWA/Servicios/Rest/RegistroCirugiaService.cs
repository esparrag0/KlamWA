using KlamWA.DTOs;
using Microsoft.AspNetCore.WebUtilities;

namespace KlamWA.Servicios.Rest;

public class RegistroCirugiaService : IRegistroCirugiaService
{
    private readonly ISesionService _sesion;
    private string BaseUrl { get; set; }

    public RegistroCirugiaService(IConfiguration configuracion, ISesionService sesion)
    {
        _sesion = sesion;
        BaseUrl = $"{configuracion["RestApi:BaseUrl"]!}/admin/cirugias";
    }

    // El administrador de la sesión queda como responsable en la bitácora
    public async Task<CirugiaRegistradaDTO> RegistrarAsync(CirugiaRegistroDTO cirugia)
    {
        string url = QueryHelpers.AddQueryString(BaseUrl, "responsable", await _sesion.ObtenerUserNameAsync());
        return await HttpUtils.DoPost<CirugiaRegistroDTO, CirugiaRegistradaDTO>(url, cirugia);
    }
}
