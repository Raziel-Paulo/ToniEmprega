using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Models;

namespace ToniEmprega.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ===============================
        // DbSets (Scaffold)
        // ===============================
        public DbSet<TipoUtilizador> TiposUtilizador { get; set; }
        public DbSet<TipoOferta> TiposOferta { get; set; }
        public DbSet<EstadoOferta> EstadosOferta { get; set; }
        public DbSet<EstadoCandidatura> EstadosCandidatura { get; set; }
        public DbSet<EstadoValidacaoUtilizador> EstadosValidacaoUtilizador { get; set; }
        public DbSet<EstadoValidacaoDocumento> EstadosValidacaoDocumento { get; set; }
        public DbSet<TipoValidacao> TiposValidacao { get; set; }
        public DbSet<DecisaoAvaliacao> DecisoesAvaliacao { get; set; }

        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<Candidatura> Candidaturas { get; set; }
        public DbSet<CandidaturaFicheiro> CandidaturasFicheiros { get; set; }
        public DbSet<AvaliacaoProfessor> AvaliacoesProfessor { get; set; }
        public DbSet<ValidacaoIdentidade> ValidacoesIdentidade { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ===============================
            // SEED DATA
            // ===============================

            builder.Entity<TipoUtilizador>().HasData(
                new TipoUtilizador { Id = 1, Nome = "Aluno" },
                new TipoUtilizador { Id = 2, Nome = "Empresa" },
                new TipoUtilizador { Id = 3, Nome = "Professor" },
                new TipoUtilizador { Id = 4, Nome = "Admin" }
            );

            builder.Entity<EstadoCandidatura>().HasData(
                new EstadoCandidatura { Id = 1, Nome = "Pendente" },
                new EstadoCandidatura { Id = 2, Nome = "Aceite" },
                new EstadoCandidatura { Id = 3, Nome = "Rejeitada" }
            );

            builder.Entity<EstadoOferta>().HasData(
                new EstadoOferta { Id = 1, Nome = "Ativa" },
                new EstadoOferta { Id = 2, Nome = "Expirada" },
                new EstadoOferta { Id = 3, Nome = "Removida" }
            );

            builder.Entity<EstadoValidacaoUtilizador>().HasData(
                new EstadoValidacaoUtilizador { Id = 1, Nome = "Pendente" },
                new EstadoValidacaoUtilizador { Id = 2, Nome = "Aprovado" },
                new EstadoValidacaoUtilizador { Id = 3, Nome = "Rejeitado" }
            );

            builder.Entity<DecisaoAvaliacao>().HasData(
                new DecisaoAvaliacao { Id = 1, Nome = "Aprovado" },
                new DecisaoAvaliacao { Id = 2, Nome = "Rejeitado" },
                new DecisaoAvaliacao { Id = 3, Nome = "Rever" }
            );

            // ===============================
            // RELACIONAMENTOS (ANTI-CASCADE)
            // ===============================

            // Candidatura -> Oferta
            builder.Entity<Candidatura>()
                .HasOne(c => c.Oferta)
                .WithMany(o => o.Candidaturas)
                .HasForeignKey(c => c.OfertaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Candidatura -> ApplicationUser
            builder.Entity<Candidatura>()
                .HasOne(c => c.User)
                .WithMany(u => u.Candidaturas)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // CandidaturaFicheiro -> Candidatura
            builder.Entity<CandidaturaFicheiro>()
                .HasOne(cf => cf.Candidatura)
                .WithMany(c => c.Ficheiros)
                .HasForeignKey(cf => cf.CandidaturaId)
                .OnDelete(DeleteBehavior.Cascade); // aqui PODE ter cascade

            // Oferta -> ApplicationUser
            builder.Entity<Oferta>()
                .HasOne(o => o.User)
                .WithMany(u => u.Ofertas)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // AvaliacaoProfessor -> Candidatura
            builder.Entity<AvaliacaoProfessor>()
                .HasOne(a => a.Candidatura)
                .WithMany(c => c.Avaliacoes)
                .HasForeignKey(a => a.CandidaturaId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
