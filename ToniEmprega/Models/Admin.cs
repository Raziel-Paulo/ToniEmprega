// Models/Admin.cs (NOVO)
namespace ToniEmprega.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;
    }
}