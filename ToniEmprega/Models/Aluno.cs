// Models/Aluno.cs (ATUALIZADO)
namespace ToniEmprega.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;

        public string Curso { get; set; } = string.Empty;
        public string Ano_Letivo { get; set; } = string.Empty;
        public string Numero_Aluno { get; set; } = string.Empty;

        // Navegação
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    }
}