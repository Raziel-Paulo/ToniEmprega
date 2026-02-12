// Models/TipoOferta.cs
namespace ToniEmprega.Models
{
    public class TipoOferta
    {
        public int Id { get; set; }
        public string Designacao { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public ICollection<Oferta> Ofertas { get; set; } = new List<Oferta>();
    }
}