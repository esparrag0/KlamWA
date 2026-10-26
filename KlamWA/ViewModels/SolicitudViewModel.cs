namespace KlamWA.ViewModels {
    public class SolicitudViewModel {
        public int Id { get; set; }

        public string Cliente { get; set; } = "";

        public string Paciente { get; set; } = "";

        public string Procedimiento { get; set; } = "";

        public DateTime FechaSolicitada { get; set; }

        public string Estado { get; set; } = "";

        public string Codigo {
            get {
                return $"SOL-{Id:000}";
            }
        }
    }
}
