using Microsoft.AspNetCore.Identity;

namespace ToniEmprega.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int TipoUtilizadorId { get; set; }
        public TipoUtilizador TipoUtilizador { get; set; }

        public int? EstadoValidacaoUtilizadorId { get; set; }
        public EstadoValidacaoUtilizador EstadoValidacaoUtilizador { get; set; }

        public ICollection<Candidatura> CandidaturasAluno { get; set; }
        public ICollection<Oferta> OfertasEmpresa { get; set; }
        public ICollection<AvaliacaoProfessor> AvaliacoesProfessor { get; set; }
        public ICollection<ValidacaoIdentidade> ValidacoesIdentidade { get; set; }
    }
}