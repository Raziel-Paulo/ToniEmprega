// Models/EstadoValidacaoUtilizador.cs
namespace ToniEmprega.Models
{
    public class EstadoValidacaoUtilizador
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public ICollection<Utilizador> Utilizadores { get; set; } = new List<Utilizador>();
    }
}