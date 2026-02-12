// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Models;

namespace ToniEmprega.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<TipoUtilizador> TipoUtilizadores { get; set; }
        public DbSet<EstadoValidacaoUtilizador> EstadoValidacaoUtilizadores { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<TipoOferta> TipoOfertas { get; set; }
        public DbSet<EstadoOferta> EstadoOfertas { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<EstadoCandidatura> EstadoCandidaturas { get; set; }
        public DbSet<Candidatura> Candidaturas { get; set; }
        public DbSet<AvaliacaoProfessor> AvaliacoesProfessores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aluno>().ToTable("Alunos");
            modelBuilder.Entity<Professor>().ToTable("Professores");
            modelBuilder.Entity<Empresa>().ToTable("Empresas");

            // Seed data
            modelBuilder.Entity<TipoUtilizador>().HasData(
                new TipoUtilizador { Id = 1, Designacao = "Aluno" },
                new TipoUtilizador { Id = 2, Designacao = "Professor" },
                new TipoUtilizador { Id = 3, Designacao = "Empresa" },
                new TipoUtilizador { Id = 4, Designacao = "Administrador" }
            );

            modelBuilder.Entity<EstadoValidacaoUtilizador>().HasData(
                new EstadoValidacaoUtilizador { Id = 1, Designacao = "Pendente" },
                new EstadoValidacaoUtilizador { Id = 2, Designacao = "Aprovado" },
                new EstadoValidacaoUtilizador { Id = 3, Designacao = "Rejeitado" }
            );

            modelBuilder.Entity<TipoOferta>().HasData(
                new TipoOferta { Id = 1, Designacao = "Estágio", Descricao = "Estágio curricular" },
                new TipoOferta { Id = 2, Designacao = "Emprego", Descricao = "Contrato de trabalho" },
                new TipoOferta { Id = 3, Designacao = "Projeto", Descricao = "Projeto temporário" }
            );

            modelBuilder.Entity<EstadoOferta>().HasData(
                new EstadoOferta { Id = 1, Designacao = "Ativa" },
                new EstadoOferta { Id = 2, Designacao = "Expirada" },
                new EstadoOferta { Id = 3, Designacao = "Preenchida" }
            );

            modelBuilder.Entity<EstadoCandidatura>().HasData(
                new EstadoCandidatura { Id = 1, Designacao = "Pendente" },
                new EstadoCandidatura { Id = 2, Designacao = "Em Análise" },
                new EstadoCandidatura { Id = 3, Designacao = "Aprovada" },
                new EstadoCandidatura { Id = 4, Designacao = "Rejeitada" }
            );
        }
    }
}