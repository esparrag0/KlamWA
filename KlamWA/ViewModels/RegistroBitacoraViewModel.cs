namespace KlamWA.ViewModels {
    public class RegistroBitacoraViewModel {
        public int Id { get; set; }

        public DateTime FechaHora { get; set; }

        public string Cirugia { get; set; } = "";

        public string Accion { get; set; } = "";

        public string Responsable { get; set; } = "";

        public string Detalle { get; set; } = "";
    }
}
