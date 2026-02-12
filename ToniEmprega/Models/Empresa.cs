// Models/Empresa.cs (ATUALIZADO)
namespace ToniEmprega.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;

        public string Nome_Empresa { get; set; } = string.Empty;
        public string Nif { get; set; } = string.Empty;
        public string Morada { get; set; } = string.Empty;
        public string Site_Empresa { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;

        // Navegação
        public ICollection<Oferta> Ofertas { get; set; } = new List<Oferta>();
    }
}