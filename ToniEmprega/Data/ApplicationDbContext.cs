// Data/ApplicationDbContext.cs - VERSÃO LIMPA E DEFINITIVA
using Microsoft.EntityFrameworkCore;
using ToniEmprega.Models;

namespace ToniEmprega.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets - TODOS os modelos
        public DbSet<TipoUtilizador> TipoUtilizadores { get; set; }
        public DbSet<EstadoValidacaoUtilizador> EstadoValidacaoUtilizadores { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<UtilizadorNormal> UtilizadoresNormais { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<TipoOferta> TipoOfertas { get; set; }
        public DbSet<EstadoOferta> EstadoOfertas { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
        public DbSet<EstadoCandidatura> EstadoCandidaturas { get; set; }
        public DbSet<Candidatura> Candidaturas { get; set; }
        public DbSet<AvaliacaoProfessor> AvaliacoesProfessores { get; set; }
        public DbSet<TipoValidacao> TipoValidacoes { get; set; }
        public DbSet<EstadoValidacaoDocumento> EstadoValidacaoDocumentos { get; set; }
        public DbSet<ValidacaoIdentidade> ValidacoesIdentidade { get; set; }
        public DbSet<DecisaoAvaliacao> DecisaoAvaliacoes { get; set; }
        public DbSet<CandidaturaFicheiro> CandidaturaFicheiros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar nomes de tabelas
            modelBuilder.Entity<Utilizador>().ToTable("Utilizadores");
            modelBuilder.Entity<Aluno>().ToTable("Alunos");
            modelBuilder.Entity<Professor>().ToTable("Professores");
            modelBuilder.Entity<Empresa>().ToTable("Empresas");
            modelBuilder.Entity<UtilizadorNormal>().ToTable("UtilizadoresNormais");
            modelBuilder.Entity<Admin>().ToTable("Admins");
            modelBuilder.Entity<TipoUtilizador>().ToTable("TiposUtilizador");
            modelBuilder.Entity<EstadoValidacaoUtilizador>().ToTable("EstadosValidacaoUtilizador");
            modelBuilder.Entity<TipoValidacao>().ToTable("TiposValidacao");
            modelBuilder.Entity<EstadoValidacaoDocumento>().ToTable("EstadosValidacaoDocumento");
            modelBuilder.Entity<ValidacaoIdentidade>().ToTable("ValidacoesIdentidade");
            modelBuilder.Entity<TipoOferta>().ToTable("TiposOferta");
            modelBuilder.Entity<EstadoOferta>().ToTable("EstadosOferta");
            modelBuilder.Entity<Oferta>().ToTable("Ofertas");
            modelBuilder.Entity<EstadoCandidatura>().ToTable("EstadosCandidatura");
            modelBuilder.Entity<Candidatura>().ToTable("Candidaturas");
            modelBuilder.Entity<CandidaturaFicheiro>().ToTable("CandidaturasFicheiros");
            modelBuilder.Entity<DecisaoAvaliacao>().ToTable("DecisoesAvaliacao");
            modelBuilder.Entity<AvaliacaoProfessor>().ToTable("AvaliacoesProfessores");

            // Relações com RESTRICT para evitar ciclos
            modelBuilder.Entity<Utilizador>()
                .HasOne(u => u.TipoUtilizador)
                .WithMany(t => t.Utilizadores)
                .HasForeignKey(u => u.Id_Tipo_Utilizador)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Utilizador>()
                .HasOne(u => u.EstadoValidacao)
                .WithMany(e => e.Utilizadores)
                .HasForeignKey(u => u.Id_Estado_Validacao_Utilizador)
                .OnDelete(DeleteBehavior.SetNull);

            // Herança - TPT
            modelBuilder.Entity<Aluno>()
                .HasOne(a => a.Utilizador)
                .WithOne()
                .HasForeignKey<Aluno>(a => a.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Professor>()
                .HasOne(p => p.Utilizador)
                .WithOne()
                .HasForeignKey<Professor>(p => p.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Empresa>()
                .HasOne(e => e.Utilizador)
                .WithOne()
                .HasForeignKey<Empresa>(e => e.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UtilizadorNormal>()
                .HasOne(un => un.Utilizador)
                .WithOne()
                .HasForeignKey<UtilizadorNormal>(un => un.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Admin>()
                .HasOne(a => a.Utilizador)
                .WithOne()
                .HasForeignKey<Admin>(a => a.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            // Ofertas
            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.Empresa)
                .WithMany(e => e.Ofertas)
                .HasForeignKey(o => o.Id_Empresa)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.TipoOferta)
                .WithMany(t => t.Ofertas)
                .HasForeignKey(o => o.Id_Tipo_Oferta)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Oferta>()
                .HasOne(o => o.EstadoOferta)
                .WithMany(e => e.Ofertas)
                .HasForeignKey(o => o.Id_Estado_Oferta)
                .OnDelete(DeleteBehavior.SetNull);

            // Candidaturas - RESTRICT em ambos
            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Oferta)
                .WithMany(o => o.Candidaturas)
                .HasForeignKey(c => c.Id_Oferta)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.Aluno)
                .WithMany(a => a.Candidaturas)
                .HasForeignKey(c => c.Id_Aluno)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.EstadoCandidatura)
                .WithMany(e => e.Candidaturas)
                .HasForeignKey(c => c.Id_Estado_Candidatura)
                .OnDelete(DeleteBehavior.SetNull);

            // Ficheiros
            modelBuilder.Entity<CandidaturaFicheiro>()
                .HasOne(cf => cf.Candidatura)
                .WithMany(c => c.Ficheiros)
                .HasForeignKey(cf => cf.Id_Candidatura)
                .OnDelete(DeleteBehavior.Cascade);

            // Avaliações
            modelBuilder.Entity<AvaliacaoProfessor>()
                .HasOne(a => a.Candidatura)
                .WithMany(c => c.Avaliacoes)
                .HasForeignKey(a => a.Id_Candidatura)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AvaliacaoProfessor>()
                .HasOne(a => a.Professor)
                .WithMany(p => p.Avaliacoes)
                .HasForeignKey(a => a.Id_Professor)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AvaliacaoProfessor>()
                .HasOne(a => a.DecisaoAvaliacao)
                .WithMany(d => d.Avaliacoes)
                .HasForeignKey(a => a.Id_Decisao_Avaliacao)
                .OnDelete(DeleteBehavior.SetNull);

            // Validações
            modelBuilder.Entity<ValidacaoIdentidade>()
                .HasOne(v => v.Utilizador)
                .WithMany(u => u.ValidacoesIdentidade)
                .HasForeignKey(v => v.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValidacaoIdentidade>()
                .HasOne(v => v.TipoValidacao)
                .WithMany(t => t.Validacoes)
                .HasForeignKey(v => v.Id_Tipo_Validacao)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ValidacaoIdentidade>()
                .HasOne(v => v.EstadoValidacaoDocumento)
                .WithMany(e => e.Validacoes)
                .HasForeignKey(v => v.Id_Estado_Validacao_Documento)
                .OnDelete(DeleteBehavior.SetNull);

            // SEED DATA
            modelBuilder.Entity<TipoUtilizador>().HasData(
                new TipoUtilizador { Id = 1, Designacao = "Aluno" },
                new TipoUtilizador { Id = 2, Designacao = "Professor" },
                new TipoUtilizador { Id = 3, Designacao = "Empresa" },
                new TipoUtilizador { Id = 4, Designacao = "Utilizador Normal" },
                new TipoUtilizador { Id = 5, Designacao = "Administrador" }
            );

            modelBuilder.Entity<EstadoValidacaoUtilizador>().HasData(
                new EstadoValidacaoUtilizador { Id = 1, Designacao = "Pendente" },
                new EstadoValidacaoUtilizador { Id = 2, Designacao = "Aprovado" },
                new EstadoValidacaoUtilizador { Id = 3, Designacao = "Rejeitado" }
            );

            modelBuilder.Entity<EstadoValidacaoDocumento>().HasData(
                new EstadoValidacaoDocumento { Id = 1, Designacao = "Pendente" },
                new EstadoValidacaoDocumento { Id = 2, Designacao = "Aprovado" },
                new EstadoValidacaoDocumento { Id = 3, Designacao = "Rejeitado" }
            );

            modelBuilder.Entity<TipoValidacao>().HasData(
                new TipoValidacao { Id = 1, Designacao = "Cartão de Estudante" },
                new TipoValidacao { Id = 2, Designacao = "Bilhete de Identidade" },
                new TipoValidacao { Id = 3, Designacao = "Cartão de Cidadão" }
            );

            modelBuilder.Entity<TipoOferta>().HasData(
                new TipoOferta { Id = 1, Designacao = "Estágio", Descricao = "Estágio curricular ou profissional" },
                new TipoOferta { Id = 2, Designacao = "Emprego", Descricao = "Contrato de trabalho" },
                new TipoOferta { Id = 3, Designacao = "Projeto", Descricao = "Participação em projeto" }
            );

            modelBuilder.Entity<EstadoOferta>().HasData(
                new EstadoOferta { Id = 1, Designacao = "Ativa" },
                new EstadoOferta { Id = 2, Designacao = "Expirada" },
                new EstadoOferta { Id = 3, Designacao = "Preenchida" },
                new EstadoOferta { Id = 4, Designacao = "Desativada" }
            );

            modelBuilder.Entity<EstadoCandidatura>().HasData(
                new EstadoCandidatura { Id = 1, Designacao = "Pendente" },
                new EstadoCandidatura { Id = 2, Designacao = "Em Análise" },
                new EstadoCandidatura { Id = 3, Designacao = "Aprovada" },
                new EstadoCandidatura { Id = 4, Designacao = "Rejeitada" },
                new EstadoCandidatura { Id = 5, Designacao = "Cancelada" }
            );

            modelBuilder.Entity<DecisaoAvaliacao>().HasData(
                new DecisaoAvaliacao { Id = 1, Designacao = "Aprovado" },
                new DecisaoAvaliacao { Id = 2, Designacao = "Rejeitado" },
                new DecisaoAvaliacao { Id = 3, Designacao = "Necessita de Revisão" }
            );
        }
    }
}