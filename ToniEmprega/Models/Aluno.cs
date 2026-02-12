// Models/Aluno.cs
namespace ToniEmprega.Models
{
    public class Aluno
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;
        public string Curso { get; set; } = string.Empty;
        public string AnoLetivo { get; set; } = string.Empty;
        public string NumeroAluno { get; set; } = string.Empty;
    }
}