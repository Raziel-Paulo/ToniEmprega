// Models/Professor.cs (ATUALIZADO)
namespace ToniEmprega.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;

        public string Departamento { get; set; } = string.Empty;
        public string Numero_Professor { get; set; } = string.Empty;

        // Navegação
        public ICollection<AvaliacaoProfessor> Avaliacoes { get; set; } = new List<AvaliacaoProfessor>();
    }
}