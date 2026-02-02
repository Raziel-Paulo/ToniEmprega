namespace ToniEmprega.ViewModels
{
    public class HomeDashboardVM
    {
        public string Role { get; set; }
        public int TotalOfertas { get; set; }
        public int MinhasOfertas { get; set; }
        public int MinhasCandidaturas { get; set; }
        public int CandidaturasPendentes { get; set; }
        public string EstadoValidacao { get; set; }
    }
}
