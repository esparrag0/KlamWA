using System.ComponentModel.DataAnnotations;

namespace KlamWA.ViewModels {
    public class RechazoViewModel {
        public string DocumentoObservado { get; set; } = "";

        [Required(ErrorMessage = "El motivo es obligatorio")]
        [StringLength(200, ErrorMessage = "Máximo 200 caracteres")]
        public string Motivo { get; set; } = "";

        public bool HabilitarResubida { get; set; } = true;
    }
}
