using System.ComponentModel.DataAnnotations;
using KlamWA.Components.Shared;

namespace KlamWA.ViewModels;

// Validación de frontend (solo ayuda de UX). Refleja las reglas de
// PacienteParticularBLImpl y ClinicaHospitalBLImpl (Java): nombre obligatorio,
// longitudes máximas, formato de correo, DNI de 8 dígitos y RUC de 11 dígitos.
// La BL sigue siendo la defensa real (incluida la unicidad de DNI/RUC).
public class ClienteViewModel : IValidatableObject
{
    public int Id { get; set; }

    public string Tipo { get; set; } = DatosClientes.Particular;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
    public string Nombre { get; set; } = "";

    [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
    public string Direccion { get; set; } = "";

    [StringLength(120, ErrorMessage = "El correo no puede exceder 120 caracteres")]
    public string Email { get; set; } = "";

    [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
    public string Telefono { get; set; } = "";

    // Particular
    public string Dni { get; set; } = "";
    public bool PagoConfirmado { get; set; }

    // Clínica / hospital
    public string Ruc { get; set; } = "";
    public bool TieneConsignacion { get; set; }
    public string PeriodoCredito { get; set; } = "";

    public ClienteViewModel() { }

    public ClienteViewModel(ClienteMock c)
    {
        Id = c.Id;
        Tipo = c.Tipo;
        Nombre = c.Nombre;
        Direccion = c.Direccion;
        Email = c.Email;
        Telefono = c.Telefono;
        Dni = c.Dni;
        PagoConfirmado = c.PagoConfirmado;
        Ruc = c.Ruc;
        TieneConsignacion = c.TieneConsignacion;
        PeriodoCredito = c.PeriodoCredito;
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (!string.IsNullOrWhiteSpace(Email) && (!Email.Contains('@') || !Email.Contains('.')))
            yield return new ValidationResult(
                "El correo de contacto tiene un formato inválido", new[] { nameof(Email) });

        if (Tipo == DatosClientes.Particular)
        {
            if (Dni.Length != 8 || !Dni.All(char.IsAsciiDigit))
                yield return new ValidationResult(
                    "El DNI debe tener exactamente 8 dígitos", new[] { nameof(Dni) });
        }
        else
        {
            if (Ruc.Length != 11 || !Ruc.All(char.IsAsciiDigit))
                yield return new ValidationResult(
                    "El RUC debe tener exactamente 11 dígitos", new[] { nameof(Ruc) });
        }
    }
}
