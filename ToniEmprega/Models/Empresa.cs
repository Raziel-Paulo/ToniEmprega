// Models/Empresa.cs
namespace ToniEmprega.Models
{
    public class Empresa
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;
        public string NomeEmpresa { get; set; } = string.Empty;
        public string NIF { get; set; } = string.Empty;
        public string Morada { get; set; } = string.Empty;
        public string SiteEmpresa { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
    }
}