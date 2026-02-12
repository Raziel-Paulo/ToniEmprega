// Models/Candidatura.cs
namespace ToniEmprega.Models
{
    public class Candidatura
    {
        public Candidatura()
        {
            DataCandidatura = DateTime.Now;
        }

        public int Id { get; set; }
        public int Id_Oferta { get; set; }
        public Oferta Oferta { get; set; } = null!;
        public int Id_Aluno { get; set; }
        public Aluno Aluno { get; set; } = null!;
        public int? Id_Estado_Candidatura { get; set; }
        public EstadoCandidatura? EstadoCandidatura { get; set; }
        public DateTime DataCandidatura { get; set; }
        public string? Mensagem { get; set; }
    }
}