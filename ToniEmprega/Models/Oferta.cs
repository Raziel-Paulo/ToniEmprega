// Models/Oferta.cs
namespace ToniEmprega.Models
{
    public class Oferta
    {
        public int Id { get; set; }

        public int Id_Empresa { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Empresa Empresa { get; set; } = null!;

        public int? Id_Tipo_Oferta { get; set; }
        public TipoOferta? TipoOferta { get; set; }

        public int? Id_Estado_Oferta { get; set; }
        public EstadoOferta? EstadoOferta { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;

        // 🗺️ NOVO: Coordenadas GPS para o mapa
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public DateTime Data_Publicacao { get; set; } = DateTime.Now;
        public DateTime? Data_Expiracao { get; set; }

        // Navegação
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    }
}