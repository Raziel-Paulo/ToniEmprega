// Models/EstadoValidacaoDocumento.cs (NOVO)
namespace ToniEmprega.Models
{
    public class EstadoValidacaoDocumento
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty; // Pendente, Aprovado, Rejeitado
        public ICollection<ValidacaoIdentidade> Validacoes { get; set; } = new List<ValidacaoIdentidade>();
    }
}