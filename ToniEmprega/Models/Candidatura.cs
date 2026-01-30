using Microsoft.AspNetCore.Identity;
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

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Required]
        public string AlunoId { get; set; }
        public ApplicationUser Aluno { get; set; }

        [Required, ForeignKey("EstadoCandidatura")]
        public int EstadoCandidaturaId { get; set; }
        public EstadoCandidatura EstadoCandidatura { get; set; }

        [Required]
        public DateTime DataSubmissao { get; set; }

        public ICollection<CandidaturaFicheiro> Ficheiros { get; set; } = new List<CandidaturaFicheiro>();

        public ICollection<AvaliacaoProfessor> Avaliacoes { get; set; } = new List<AvaliacaoProfessor>();
    }
}
