using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ToniEmprega.Models
{
    [Table("Ofertas")]
    public class Oferta
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public IdentityUser User { get; set; }

        [Required, StringLength(100)]
        public string Titulo { get; set; }

        [Required, StringLength(500)]
        public string Descricao { get; set; }

        [StringLength(300)]
        public string Requisitos { get; set; }

        [Required, StringLength(100)]
        public string Localizacao { get; set; }

        [Required, ForeignKey("TipoOferta")]
        public int TipoOfertaId { get; set; }
        public TipoOferta TipoOferta { get; set; }

        [Required, ForeignKey("EstadoOferta")]
        public int EstadoOfertaId { get; set; }
        public EstadoOferta EstadoOferta { get; set; }

        [Required]
        public string EmpresaId { get; set; }
        public IdentityUser Empresa { get; set; }

        [Required]
        public DateTime DataCriacao { get; set; }

        public DateTime? DataExpiracao { get; set; }
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    }
}
