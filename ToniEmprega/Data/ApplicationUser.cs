using Microsoft.AspNetCore.Identity;

namespace ToniEmprega.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int TipoUtilizadorId { get; set; }
        public TipoUtilizador TipoUtilizador { get; set; }

        public int? EstadoValidacaoUtilizadorId { get; set; }
        public EstadoValidacaoUtilizador EstadoValidacaoUtilizador { get; set; }

        public ICollection<ValidacaoIdentidade> Validacoes { get; set; }
        public ICollection<Candidatura> Candidaturas { get; set; }
        public ICollection<AvaliacaoProfessor> Avaliacoes { get; set; }
    }
}