// Models/AvaliacaoProfessor.cs (ATUALIZADO)
namespace ToniEmprega.Models
{
    public class AvaliacaoProfessor
    {
        public int Id { get; set; }

        public int Id_Candidatura { get; set; }
        public Candidatura Candidatura { get; set; } = null!;

        public int Id_Professor { get; set; }
        public Professor Professor { get; set; } = null!;

        public int? Id_Decisao_Avaliacao { get; set; }
        public DecisaoAvaliacao? DecisaoAvaliacao { get; set; }

        public string Comentarios { get; set; } = string.Empty;
        public DateTime Data_Avaliacao { get; set; } = DateTime.Now;
    }
}