using Microsoft.AspNetCore.Components.Forms;

namespace KlamWA.Servicios;

/// <summary>Comprobación temprana; Java verifica nuevamente el contenido real.</summary>
public static class ArchivoDocumentoSeleccion
{
    public const long MaximoBytes = 5L * 1024 * 1024;

    public static string? Validar(IBrowserFile archivo)
    {
        if (archivo.Size == 0)
            return "El archivo está vacío. Selecciona un PDF, PNG o JPEG con contenido.";
        if (archivo.Size > MaximoBytes)
            return "El archivo supera el límite de 5 MB. Selecciona uno más pequeño.";
        var extension = Path.GetExtension(archivo.Name);
        if (!extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase)
            && !extension.Equals(".png", StringComparison.OrdinalIgnoreCase)
            && !extension.Equals(".jpg", StringComparison.OrdinalIgnoreCase)
            && !extension.Equals(".jpeg", StringComparison.OrdinalIgnoreCase))
            return "Solo se admiten archivos PDF, PNG o JPEG.";
        return null;
    }
}
