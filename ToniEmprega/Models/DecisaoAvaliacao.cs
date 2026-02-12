// Models/DecisaoAvaliacao.cs (NOVO)
namespace ToniEmprega.Models
{
    public class DecisaoAvaliacao
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty; // Aprovado, Rejeitado, Necessita Revisão
        public ICollection<AvaliacaoProfessor> Avaliacoes { get; set; } = new List<AvaliacaoProfessor>();
    }
}