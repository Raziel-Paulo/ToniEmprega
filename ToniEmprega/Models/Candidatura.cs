using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ToniEmprega.Models
{
    [Table("Candidaturas")]
    public class Candidatura
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Oferta")]
        public int OfertaId { get; set; }
        public Oferta Oferta { get; set; }

        [Required]
        public string AlunoId { get; set; }
        public ApplicationUser Aluno { get; set; }

        [Required, ForeignKey("EstadoCandidatura")]
        public int EstadoCandidaturaId { get; set; }
        public EstadoCandidatura EstadoCandidatura { get; set; }

        [Required]
        public DateTime DataSubmissao { get; set; }
    }
}
