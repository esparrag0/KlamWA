using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using KlamWA.Dtos;

namespace KlamWA.Services;

// Sesión del usuario que ingresó por /login. Se guarda cifrada en el sessionStorage de la pestaña
// (ProtectedSessionStorage) para sobrevivir a una recarga (F5); se borra al cerrar sesión o la pestaña.
// Solo guarda quién es y su token: las credenciales las valida el REST/BL y los permisos los verifica el servidor.
public class SesionUsuario
{
    private const string Clave = "klam.sesion";
    private readonly ProtectedSessionStorage _almacen;
    private readonly AuthApiService _auth;

    public SesionUsuario(ProtectedSessionStorage almacen, AuthApiService auth)
    {
        _almacen = almacen;
        _auth = auth;
    }

    public UsuarioSesionDto? Usuario { get; private set; }

    public bool EstaAutenticado => Usuario is not null;
    public string UserName => Usuario?.UserName ?? "";
    public string? Token => Usuario?.Token;
    public bool EsCliente => Usuario?.Rol is "CLINICA_HOSPITAL" or "PACIENTE_PARTICULAR";

    // Clave del perfil visual (DatosAcceso.Perfiles) que corresponde al rol real.
    public string? ClavePerfil => Usuario?.Rol switch
    {
        "ADMINISTRADOR" => "administrador",
        "VENDEDOR" => "vendedor",
        "TECNICO_INSTRUMENTISTA" => "instrumentista",
        "CLINICA_HOSPITAL" or "PACIENTE_PARTICULAR" => "cliente",
        _ => null
    };

    public async Task IniciarAsync(UsuarioSesionDto usuario)
    {
        Usuario = usuario;
        await _almacen.SetAsync(Clave, usuario);
    }

    // Solo con JavaScript disponible (OnAfterRender). Devuelve true si hay sesión (en memoria o guardada).
    public async Task<bool> RestaurarAsync()
    {
        if (EstaAutenticado) return true;
        try
        {
            var guardada = await _almacen.GetAsync<UsuarioSesionDto>(Clave);
            if (guardada.Success && guardada.Value is not null)
            {
                Usuario = guardada.Value;
                return true;
            }
        }
        catch (Exception) { /* valor ilegible o circuito sin JS: se trata como sin sesión */ }
        return false;
    }

    public async Task CerrarAsync()
    {
        var token = Token;
        Usuario = null;
        try { await _almacen.DeleteAsync(Clave); }
        catch (Exception) { /* circuito desconectado: no hay nada que borrar */ }
        if (!string.IsNullOrEmpty(token)) await _auth.LogoutAsync(token);
    }
}
