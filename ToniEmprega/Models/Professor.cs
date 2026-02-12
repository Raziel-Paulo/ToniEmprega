// Models/Professor.cs
namespace ToniEmprega.Models
{
    public class Professor
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;
        public string Departamento { get; set; } = string.Empty;
        public string NumeroProfessor { get; set; } = string.Empty;
    }
}