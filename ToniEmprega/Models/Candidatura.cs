// Models/Candidatura.cs
namespace ToniEmprega.Models
{
    public class Candidatura
    {
        public int Id { get; set; }

        public int Id_Oferta { get; set; }
        public Oferta Oferta { get; set; } = null!;

        public int Id_Aluno { get; set; }
        public Aluno Aluno { get; set; } = null!;

        public int? Id_Estado_Candidatura { get; set; }
        public EstadoCandidatura? EstadoCandidatura { get; set; }

        // NOVO: Estado separado para a decisão da empresa
        public int? Id_Estado_Candidatura_Empresa { get; set; }
        public EstadoCandidatura? EstadoCandidaturaEmpresa { get; set; }

        public DateTime Data_Candidatura { get; set; } = DateTime.Now;
        public string? Mensagem { get; set; }

        public ICollection<CandidaturaFicheiro> Ficheiros { get; set; } = new List<CandidaturaFicheiro>();
        public ICollection<AvaliacaoProfessor> Avaliacoes { get; set; } = new List<AvaliacaoProfessor>();
    }
}