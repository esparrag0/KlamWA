using KlamWA.ViewModels;

namespace KlamWA.Servicios;

public interface IEquipoService
{
    Task<List<EquipoOpcionViewModel>> Listar(bool soloDisponibles = false, string? categoria = null);
}
