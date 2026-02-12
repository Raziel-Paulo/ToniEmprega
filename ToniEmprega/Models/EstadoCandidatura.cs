// Models/EstadoCandidatura.cs
namespace ToniEmprega.Models
{
    public class EstadoCandidatura
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    }
}