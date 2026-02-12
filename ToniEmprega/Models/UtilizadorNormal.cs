// Models/UtilizadorNormal.cs (NOVO)
namespace ToniEmprega.Models
{
    public class UtilizadorNormal
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;
        public string Documentacao_Identificacao { get; set; } = string.Empty;
    }
}