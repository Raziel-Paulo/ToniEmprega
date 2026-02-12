// Models/TipoValidacao.cs (NOVO)
namespace ToniEmprega.Models
{
    public class TipoValidacao
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty; // Cartão de Estudante, BI/CC
        public ICollection<ValidacaoIdentidade> Validacoes { get; set; } = new List<ValidacaoIdentidade>();
    }
}