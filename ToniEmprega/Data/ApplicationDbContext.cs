using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ToniEmprega.Models;

namespace ToniEmprega.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // DbSets - APENAS UMA VEZ CADA UM
        public DbSet<TipoUtilizador> TipoUtilizadores { get; set; }
        public DbSet<EstadoValidacaoUtilizador> EstadoValidacaoUtilizadores { get; set; }
        public DbSet<Utilizador> Utilizadores { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
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
        public DbSet<Notificacao> Notificacoes { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<DocumentoValidacao> DocumentosValidacao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Converte todas as datas para UTC para consistência na base local
            var utcDateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v.Kind == DateTimeKind.Utc ? v : DateTime.SpecifyKind(v, DateTimeKind.Utc),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var utcNullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v.HasValue
                    ? (v.Value.Kind == DateTimeKind.Utc ? v.Value : DateTime.SpecifyKind(v.Value, DateTimeKind.Utc))
                    : v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);


            // Nomes de tabelas
            modelBuilder.Entity<Utilizador>().ToTable("Utilizadores");
            modelBuilder.Entity<Aluno>().ToTable("Alunos");
            modelBuilder.Entity<Professor>().ToTable("Professores");
            modelBuilder.Entity<Empresa>().ToTable("Empresas");
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
            modelBuilder.Entity<Notificacao>().ToTable("Notificacoes");
            modelBuilder.Entity<Turma>().ToTable("Turmas");
            modelBuilder.Entity<DocumentoValidacao>().ToTable("DocumentosValidacao");

            // Relações
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

            // Herança TPT
            modelBuilder.Entity<Aluno>()
                .HasOne(a => a.Utilizador)
                .WithOne()
                .HasForeignKey<Aluno>(a => a.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            // Aluno -> Turma
            modelBuilder.Entity<Aluno>()
                .HasOne(a => a.Turma)
                .WithMany(t => t.Alunos)
                .HasForeignKey(a => a.Id_Turma)
                .OnDelete(DeleteBehavior.SetNull);


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

            // Candidaturas
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

            modelBuilder.Entity<Candidatura>()
                .HasOne(c => c.EstadoCandidaturaEmpresa)
                .WithMany()
                .HasForeignKey(c => c.Id_Estado_Candidatura_Empresa)
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

            // Notificações
            modelBuilder.Entity<Notificacao>()
                .HasOne(n => n.Utilizador)
                .WithMany()
                .HasForeignKey(n => n.Id_Utilizador)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DocumentoValidacao>()
                .HasOne(d => d.ValidacaoIdentidade)
                .WithMany(v => v.Documentos)
                .HasForeignKey(d => d.Id_Validacao_Identidade)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ValidacaoIdentidade>()
                .HasIndex(v => v.Id_Utilizador)
                .IsUnique();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(utcDateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(utcNullableDateTimeConverter);
                    }
                }
            }

            // ============================================
            // SEED DATA
            // ============================================

            // Tipos de Utilizador
            modelBuilder.Entity<TipoUtilizador>().HasData(
                new TipoUtilizador { Id = 1, Designacao = "Aluno" },
                new TipoUtilizador { Id = 2, Designacao = "Professor" },
                new TipoUtilizador { Id = 3, Designacao = "Empresa" },
                new TipoUtilizador { Id = 5, Designacao = "Administrador" }
            );

            // Estados de Validação
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

            // Tipos de Validação
            modelBuilder.Entity<TipoValidacao>().HasData(
                new TipoValidacao { Id = 1, Designacao = "Cartão de Estudante" },
                new TipoValidacao { Id = 2, Designacao = "Bilhete de Identidade" },
                new TipoValidacao { Id = 3, Designacao = "Cartão de Cidadão" }
            );

            // Tipos de Oferta
            modelBuilder.Entity<TipoOferta>().HasData(
                new TipoOferta { Id = 1, Designacao = "Estágio", Descricao = "Estágio curricular ou profissional" },
                new TipoOferta { Id = 2, Designacao = "Emprego", Descricao = "Contrato de trabalho" },
                new TipoOferta { Id = 3, Designacao = "Projeto", Descricao = "Participação em projeto" }
            );

            // Estados de Oferta
            modelBuilder.Entity<EstadoOferta>().HasData(
                new EstadoOferta { Id = 1, Designacao = "Ativa" },
                new EstadoOferta { Id = 2, Designacao = "Expirada" },
                new EstadoOferta { Id = 3, Designacao = "Preenchida" },
                new EstadoOferta { Id = 4, Designacao = "Desativada" },
                new EstadoOferta { Id = 5, Designacao = "Bloqueada" }
            );

            // Estados de Candidatura
            modelBuilder.Entity<EstadoCandidatura>().HasData(
                new EstadoCandidatura { Id = 1, Designacao = "Pendente" },
                new EstadoCandidatura { Id = 2, Designacao = "Em Análise" },
                new EstadoCandidatura { Id = 3, Designacao = "Aprovada" },
                new EstadoCandidatura { Id = 4, Designacao = "Rejeitada" },
                new EstadoCandidatura { Id = 5, Designacao = "Cancelada" },
                new EstadoCandidatura { Id = 6, Designacao = "Aprovada pelo Professor" },
                new EstadoCandidatura { Id = 7, Designacao = "Rejeitada pelo Professor" }
            );

            // Decisões de Avaliação
            modelBuilder.Entity<DecisaoAvaliacao>().HasData(
                new DecisaoAvaliacao { Id = 1, Designacao = "Aprovado" },
                new DecisaoAvaliacao { Id = 2, Designacao = "Rejeitado" },
                new DecisaoAvaliacao { Id = 3, Designacao = "Necessita de Revisão" }
            );

            // Turmas (Seed)
            modelBuilder.Entity<Turma>().HasData(
                new Turma { Id = 1, Designacao = "222" },
                new Turma { Id = 2, Designacao = "333" },
                new Turma { Id = 3, Designacao = "444" },
                new Turma { Id = 4, Designacao = "555" },
                new Turma { Id = 5, Designacao = "777" },
                new Turma { Id = 6, Designacao = "888" },
                new Turma { Id = 7, Designacao = "999" },
                new Turma { Id = 8, Designacao = "000" },
                new Turma { Id = 9, Designacao = "111" }
            );


        }
    }
}