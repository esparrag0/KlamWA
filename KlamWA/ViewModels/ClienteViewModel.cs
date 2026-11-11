using System.ComponentModel.DataAnnotations;
using KlamWA.Components.Shared;
using KlamWA.DTOs;

namespace KlamWA.ViewModels;

// Validación de frontend (solo ayuda de UX). Refleja las reglas de
// PacienteParticularBLImpl y ClinicaHospitalBLImpl (Java): nombre obligatorio,
// longitudes máximas, formato de correo, DNI de 8 dígitos y RUC de 11 dígitos.
// La BL sigue siendo la defensa real (incluida la unicidad de DNI/RUC).
public class ClienteViewModel : IValidatableObject
{
    public int Id { get; set; }
    public bool Activo { get; set; } = true;

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
    [StringLength(30, ErrorMessage = "El periodo de crédito no puede exceder 30 caracteres")]
    public string PeriodoCredito { get; set; } = "";

    public ClienteViewModel() { }

    // Edición: carga desde el detalle que devuelve gestion-clientes/{tipo}/{id}.
    public ClienteViewModel(ClienteFormDTO d)
    {
        Id = d.IdCliente;
        Activo = d.Activo;
        Tipo = d.TipoCliente == "PACIENTE_PARTICULAR" ? DatosClientes.Particular : DatosClientes.Clinica;
        Nombre = d.Nombre;
        Direccion = d.Direccion ?? "";
        Email = d.EmailContacto ?? "";
        Telefono = d.Telefono ?? "";
        Dni = d.Dni ?? "";
        PagoConfirmado = d.PagoConfirmado;
        Ruc = d.Ruc ?? "";
        TieneConsignacion = d.TieneConsignacion;
        PeriodoCredito = d.PeriodoCredito ?? "";
    }

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

    // Arma el DTO del REST para el alta. La validación real la hace la BL.
    public ClienteFormDTO ToRegistro() => new()
    {
        TipoCliente = Tipo == DatosClientes.Particular ? "PACIENTE_PARTICULAR" : "CLINICA_HOSPITAL",
        Nombre = Nombre,
        Direccion = Direccion,
        EmailContacto = Email,
        Telefono = Telefono,
        Dni = Tipo == DatosClientes.Particular ? Dni : null,
        PagoConfirmado = PagoConfirmado,
        Ruc = Tipo == DatosClientes.Particular ? null : Ruc,
        TieneConsignacion = TieneConsignacion,
        PeriodoCredito = Tipo == DatosClientes.Particular ? null : PeriodoCredito,
        Activo = Activo
    };

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
