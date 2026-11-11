using KlamWA.DTOs;

namespace KlamWA.Servicios.Rest;

public class CuentaUsuarioService : ICuentaUsuarioService
{
    private string BaseUrlLogin { get; set; }

    public CuentaUsuarioService(IConfiguration configuracion)
    {
        BaseUrlLogin = $"{configuracion["RestApi:BaseUrl"]!}/login";
    }

    public async Task<CuentaUsuarioDTO?> AutenticarAsync(CredencialesDTO credenciales)
    {
        try
        {
            return await HttpUtils.DoPost<CredencialesDTO, CuentaUsuarioDTO>(BaseUrlLogin, credenciales);
        }
        catch (ServicioException ex) when (ex.CodigoHttp == 401)
        {
            return null;
        }
    }
}
