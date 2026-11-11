namespace KlamWA.ViewModels;

public class ClienteOpcionViewModel
{
    public int Id { get; set; }

    public string Tipo { get; set; } = "";

    public string Nombre { get; set; } = "";

    public string TipoEtiqueta => Tipo == "PACIENTE_PARTICULAR" ? "Paciente particular" : "Clínica u hospital";
}

public class EquipoOpcionViewModel
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Categoria { get; set; } = "";
    public string Marca { get; set; } = "";
    public bool Disponible { get; set; }
}
