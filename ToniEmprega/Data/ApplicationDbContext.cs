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
            // Configurar TPT (Table Per Type) para herança
            modelBuilder.Entity<Aluno>().ToTable("Alunos");
            modelBuilder.Entity<Professor>().ToTable("Professores");
            modelBuilder.Entity<Empresa>().ToTable("Empresas");

            // CORREÇÃO: Configurar relacionamentos com NO ACTION para evitar ciclos de cascata

            // Utilizador -> TipoUtilizador (cascata permitida - não é parte do ciclo)
            modelBuilder.Entity<Utilizador>()
                .HasOne(u => u.TipoUtilizador)
                .WithMany(t => t.Utilizadores)
                .HasForeignKey(u => u.Id_Tipo_Utilizador)
                .OnDelete(DeleteBehavior.Restrict); // Mudado para Restrict

            // Utilizador -> EstadoValidacao (NO ACTION)
            modelBuilder.Entity<Utilizador>()
                .HasOne(u => u.EstadoValidacao)
                .WithMany(e => e.Utilizadores)
                .HasForeignKey(u => u.Id_Estado_Validacao_Utilizador)
                .OnDelete(DeleteBehavior.SetNull); // SetNull em vez de Cascata

            // Aluno -> Utilizador (NO ACTION)
            modelBuilder.Entity<Aluno>()
                .HasOne(a => a.Utilizador)
                .WithOne()
                .HasForeignKey<Aluno>(a => a.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade); // Manter cascade aqui é seguro

            // Professor -> Utilizador (NO ACTION)
            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Utilizador)
                .WithOne()
                .HasForeignKey<Professor>(p => p.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            // Empresa -> Utilizador (NO ACTION)
            modelBuilder.Entity<Empresa>()
                .HasOne(e => e.Utilizador)
                .WithOne()
                .HasForeignKey<Empresa>(e => e.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            // Oferta -> Empresa (NO ACTION)
            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.Empresa)
                .WithMany()
                .HasForeignKey(o => o.Id_Empresa)
                .OnDelete(DeleteBehavior.Restrict); // IMPORTANTE: Restrict para evitar ciclo

            // Oferta -> TipoOferta (NO ACTION)
            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.TipoOferta)
                .WithMany(t => t.Ofertas)
                .HasForeignKey(o => o.Id_Tipo_Oferta)
                .OnDelete(DeleteBehavior.SetNull);

            // Oferta -> EstadoOferta (NO ACTION)
            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.EstadoOferta)
                .WithMany(e => e.Ofertas)
                .HasForeignKey(o => o.Id_Estado_Oferta)
                .OnDelete(DeleteBehavior.SetNull);

            // Candidatura -> Oferta (NO ACTION) - ESTE É O CRÍTICO
            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Oferta)
                .WithMany()
                .HasForeignKey(c => c.Id_Oferta)
                .OnDelete(DeleteBehavior.Restrict); // IMPORTANTE: Restrict em vez de Cascade

            // Candidatura -> Aluno (NO ACTION)
            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Aluno)
                .WithMany()
                .HasForeignKey(c => c.Id_Aluno)
                .OnDelete(DeleteBehavior.Restrict); // Restrict para evitar múltiplos caminhos

            // Candidatura -> EstadoCandidatura (NO ACTION)
            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.EstadoCandidatura)
                .WithMany(e => e.Candidaturas)
                .HasForeignKey(c => c.Id_Estado_Candidatura)
                .OnDelete(DeleteBehavior.SetNull);

            // AvaliacaoProfessor -> Candidatura (NO ACTION)
            modelBuilder.Entity<AvaliacaoProfessor>()
                .HasOne(a => a.Candidatura)
                .WithMany()
                .HasForeignKey(a => a.Id_Candidatura)
                .OnDelete(DeleteBehavior.Cascade); // Cascade aqui é seguro

            // AvaliacaoProfessor -> Professor (NO ACTION)
            modelBuilder.Entity<AvaliacaoProfessor>()
                .HasOne(a => a.Professor)
                .WithMany()
                .HasForeignKey(a => a.Id_Professor)
                .OnDelete(DeleteBehavior.Restrict); // Restrict para evitar ciclo

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