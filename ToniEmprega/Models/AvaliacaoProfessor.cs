using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ToniEmprega.Models
{
    [Table("AvaliacoesProfessor")]
    public class AvaliacaoProfessor
    {
        [Key]
        public int Id { get; set; }

        [Required, ForeignKey("Candidatura")]
        public int CandidaturaId { get; set; }
        public Candidatura Candidatura { get; set; }

        [Required]
        public string ProfessorId { get; set; }
        public ApplicationUser Professor { get; set; }

        [Required, ForeignKey("Decisao")]
        public int DecisaoId { get; set; }
        public DecisaoAvaliacao Decisao { get; set; }

        [StringLength(500)]
        public string Comentario { get; set; }

        [Required]
        public DateTime DataAvaliacao { get; set; }
    }
}
