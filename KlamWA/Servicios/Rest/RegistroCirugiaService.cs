using KlamWA.DTOs;
using KlamWA.Seguridad;
using Microsoft.AspNetCore.Components.Authorization;

namespace KlamWA.Servicios.Rest;

public sealed class RegistroCirugiaService(
    HttpClient client,
    AuthenticationStateProvider autenticacion,
    SesionTokenStore tokens) : IRegistroCirugiaService
{
    public async Task<CirugiaRegistradaDTO> RegistrarAsync(CirugiaRegistroDTO cirugia) =>
        await HttpUtilsAutenticado.DoPost<CirugiaRegistroDTO, CirugiaRegistradaDTO>(
            client, "admin/cirugias", await tokens.ObtenerAsync(autenticacion), cirugia)
            ?? throw new HttpRequestException("La respuesta del registro de cirugía está vacía.");
}
