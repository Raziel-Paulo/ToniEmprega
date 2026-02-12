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
            base.OnModelCreating(modelBuilder);

            // ============================================
            // CORREÇÃO: Configurar TPT (Table Per Type) corretamente
            // As tabelas de especialização devem ser configuradas primeiro
            // ============================================

            // Configurar Utilizador como entidade base
            modelBuilder.Entity<Utilizador>(entity =>
            {
                entity.ToTable("Utilizadores");
                entity.HasKey(e => e.Id);

                // Relação com TipoUtilizador
                entity.HasOne(u => u.TipoUtilizador)
                    .WithMany(t => t.Utilizadores)
                    .HasForeignKey(u => u.Id_Tipo_Utilizador)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relação com EstadoValidacao
                entity.HasOne(u => u.EstadoValidacao)
                    .WithMany(e => e.Utilizadores)
                    .HasForeignKey(u => u.Id_Estado_Validacao_Utilizador)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Configurar Aluno - TABELA SEPARADA (TPT)
            modelBuilder.Entity<Aluno>(entity =>
            {
                entity.ToTable("Alunos");
                entity.HasKey(e => e.Id);

                entity.HasOne(a => a.Utilizador)
                    .WithOne()
                    .HasForeignKey<Aluno>(a => a.Id_Utilizador)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar Professor - TABELA SEPARADA (TPT)
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.ToTable("Professores");
                entity.HasKey(e => e.Id);

                entity.HasOne(p => p.Utilizador)
                    .WithOne()
                    .HasForeignKey<Professor>(p => p.Id_Utilizador)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configurar Empresa - TABELA SEPARADA (TPT)
            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.ToTable("Empresas");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Utilizador)
                    .WithOne()
                    .HasForeignKey<Empresa>(e => e.Id_Utilizador)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ============================================
            // Configurar Oferta e relacionamentos
            // ============================================

            modelBuilder.Entity<Oferta>(entity =>
            {
                entity.ToTable("Ofertas");
                entity.HasKey(e => e.Id);

                entity.HasOne(o => o.Empresa)
                    .WithMany()
                    .HasForeignKey(o => o.Id_Empresa)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(o => o.TipoOferta)
                    .WithMany(t => t.Ofertas)
                    .HasForeignKey(o => o.Id_Tipo_Oferta)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(o => o.EstadoOferta)
                    .WithMany(e => e.Ofertas)
                    .HasForeignKey(o => o.Id_Estado_Oferta)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ============================================
            // Configurar Candidatura - CORREÇÃO CRÍTICA
            // ============================================

            modelBuilder.Entity<Candidatura>(entity =>
            {
                entity.ToTable("Candidaturas");
                entity.HasKey(e => e.Id);

                // IMPORTANTE: Restrict em ambos para evitar ciclos
                entity.HasOne(c => c.Oferta)
                    .WithMany()
                    .HasForeignKey(c => c.Id_Oferta)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Aluno)
                    .WithMany()
                    .HasForeignKey(c => c.Id_Aluno)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.EstadoCandidatura)
                    .WithMany(e => e.Candidaturas)
                    .HasForeignKey(c => c.Id_Estado_Candidatura)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // ============================================
            // Configurar AvaliacaoProfessor
            // ============================================

            modelBuilder.Entity<AvaliacaoProfessor>(entity =>
            {
                entity.ToTable("AvaliacoesProfessores");
                entity.HasKey(e => e.Id);

                entity.HasOne(a => a.Candidatura)
                    .WithMany()
                    .HasForeignKey(a => a.Id_Candidatura)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.Professor)
                    .WithMany()
                    .HasForeignKey(a => a.Id_Professor)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ============================================
            // Seed Data - Dados iniciais
            // ============================================

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