// Models/Turma.cs
namespace ToniEmprega.Models
{
    public class Turma
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty;

        // Navegação
        public ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
    }
}