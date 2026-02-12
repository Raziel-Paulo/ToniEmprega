// Models/EstadoOferta.cs
namespace ToniEmprega.Models
{
    public class EstadoOferta
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public ICollection<Oferta> Ofertas { get; set; } = new List<Oferta>();
    }
}