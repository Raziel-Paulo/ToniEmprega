using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToniEmprega.Data.Migrations
{
    /// <inheritdoc />
    public partial class Ajustes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Candidaturas_Id_Candidatura",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Professores_Id_Professor",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_EstadoCandidaturas_Id_Estado_Candidatura",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_EstadoOfertas_Id_Estado_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_TipoOfertas_Id_Tipo_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_EstadoValidacaoUtilizadores_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_TipoUtilizadores_Id_Tipo_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoUtilizadores",
                table: "TipoUtilizadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TipoOfertas",
                table: "TipoOfertas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadoValidacaoUtilizadores",
                table: "EstadoValidacaoUtilizadores");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadoOfertas",
                table: "EstadoOfertas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadoCandidaturas",
                table: "EstadoCandidaturas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvaliacoesProfessores",
                table: "AvaliacoesProfessores");

            migrationBuilder.RenameTable(
                name: "TipoUtilizadores",
                newName: "Tipos_Utilizador");

            migrationBuilder.RenameTable(
                name: "TipoOfertas",
                newName: "Tipos_Oferta");

            migrationBuilder.RenameTable(
                name: "EstadoValidacaoUtilizadores",
                newName: "Estados_Validacao_Utilizador");

            migrationBuilder.RenameTable(
                name: "EstadoOfertas",
                newName: "Estados_Oferta");

            migrationBuilder.RenameTable(
                name: "EstadoCandidaturas",
                newName: "Estados_Candidatura");

            migrationBuilder.RenameTable(
                name: "AvaliacoesProfessores",
                newName: "Avaliacoes_Professor");

            migrationBuilder.RenameColumn(
                name: "PalavraPasse",
                table: "Utilizadores",
                newName: "Palavra_Passe");

            migrationBuilder.RenameColumn(
                name: "DataRegisto",
                table: "Utilizadores",
                newName: "Data_Registro");

            migrationBuilder.RenameColumn(
                name: "DataNascimento",
                table: "Utilizadores",
                newName: "Data_Nascimento");

            migrationBuilder.RenameColumn(
                name: "NumeroProfessor",
                table: "Professores",
                newName: "Numero_Professor");

            migrationBuilder.RenameColumn(
                name: "DataPublicacao",
                table: "Ofertas",
                newName: "Data_Publicacao");

            migrationBuilder.RenameColumn(
                name: "DataExpiracao",
                table: "Ofertas",
                newName: "Data_Expiracao");

            migrationBuilder.RenameColumn(
                name: "NIF",
                table: "Empresas",
                newName: "Nif");

            migrationBuilder.RenameColumn(
                name: "SiteEmpresa",
                table: "Empresas",
                newName: "Site_Empresa");

            migrationBuilder.RenameColumn(
                name: "NomeEmpresa",
                table: "Empresas",
                newName: "Nome_Empresa");

            migrationBuilder.RenameColumn(
                name: "DataCandidatura",
                table: "Candidaturas",
                newName: "Data_Candidatura");

            migrationBuilder.RenameColumn(
                name: "NumeroAluno",
                table: "Alunos",
                newName: "Numero_Aluno");

            migrationBuilder.RenameColumn(
                name: "AnoLetivo",
                table: "Alunos",
                newName: "Ano_Letivo");

            migrationBuilder.RenameColumn(
                name: "DataAvaliacao",
                table: "Avaliacoes_Professor",
                newName: "Data_Avaliacao");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesProfessores_Id_Professor",
                table: "Avaliacoes_Professor",
                newName: "IX_Avaliacoes_Professor_Id_Professor");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesProfessores_Id_Candidatura",
                table: "Avaliacoes_Professor",
                newName: "IX_Avaliacoes_Professor_Id_Candidatura");

            migrationBuilder.AddColumn<int>(
                name: "Id_Decisao_Avaliacao",
                table: "Avaliacoes_Professor",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tipos_Utilizador",
                table: "Tipos_Utilizador",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tipos_Oferta",
                table: "Tipos_Oferta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estados_Validacao_Utilizador",
                table: "Estados_Validacao_Utilizador",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estados_Oferta",
                table: "Estados_Oferta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Estados_Candidatura",
                table: "Estados_Candidatura",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Avaliacoes_Professor",
                table: "Avaliacoes_Professor",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidaturas_Ficheiros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Candidatura = table.Column<int>(type: "int", nullable: false),
                    Tipo_Ficheiro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nome_Ficheiro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Caminho_Ficheiro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Upload = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas_Ficheiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidaturas_Ficheiros_Candidaturas_Id_Candidatura",
                        column: x => x.Id_Candidatura,
                        principalTable: "Candidaturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Decisoes_Avaliacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decisoes_Avaliacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Estados_Validacao_Documento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estados_Validacao_Documento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipos_Validacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos_Validacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores_Normais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Documentacao_Identificacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores_Normais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilizadores_Normais_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Validacoes_Identidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Id_Tipo_Validacao = table.Column<int>(type: "int", nullable: false),
                    Ficheiro_Prova = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Validacao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Id_Estado_Validacao_Documento = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Validacoes_Identidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Validacoes_Identidade_Estados_Validacao_Documento_Id_Estado_Validacao_Documento",
                        column: x => x.Id_Estado_Validacao_Documento,
                        principalTable: "Estados_Validacao_Documento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Validacoes_Identidade_Tipos_Validacao_Id_Tipo_Validacao",
                        column: x => x.Id_Tipo_Validacao,
                        principalTable: "Tipos_Validacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Validacoes_Identidade_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Decisoes_Avaliacao",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Aprovado" },
                    { 2, "Rejeitado" },
                    { 3, "Necessita de Revisão" }
                });

            migrationBuilder.InsertData(
                table: "Estados_Candidatura",
                columns: new[] { "Id", "Designacao" },
                values: new object[] { 5, "Cancelada" });

            migrationBuilder.InsertData(
                table: "Estados_Oferta",
                columns: new[] { "Id", "Designacao" },
                values: new object[] { 4, "Desativada" });

            migrationBuilder.InsertData(
                table: "Estados_Validacao_Documento",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Aprovado" },
                    { 3, "Rejeitado" }
                });

            migrationBuilder.UpdateData(
                table: "Tipos_Oferta",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descricao",
                value: "Estágio curricular ou profissional");

            migrationBuilder.UpdateData(
                table: "Tipos_Oferta",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descricao",
                value: "Participação em projeto");

            migrationBuilder.UpdateData(
                table: "Tipos_Utilizador",
                keyColumn: "Id",
                keyValue: 4,
                column: "Designacao",
                value: "Utilizador Normal");

            migrationBuilder.InsertData(
                table: "Tipos_Utilizador",
                columns: new[] { "Id", "Designacao" },
                values: new object[] { 5, "Administrador" });

            migrationBuilder.InsertData(
                table: "Tipos_Validacao",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Cartão de Estudante" },
                    { 2, "Bilhete de Identidade" },
                    { 3, "Cartão de Cidadão" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_Professor_Id_Decisao_Avaliacao",
                table: "Avaliacoes_Professor",
                column: "Id_Decisao_Avaliacao");

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Id_Utilizador",
                table: "Admins",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_Ficheiros_Id_Candidatura",
                table: "Candidaturas_Ficheiros",
                column: "Id_Candidatura");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_Normais_Id_Utilizador",
                table: "Utilizadores_Normais",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Validacoes_Identidade_Id_Estado_Validacao_Documento",
                table: "Validacoes_Identidade",
                column: "Id_Estado_Validacao_Documento");

            migrationBuilder.CreateIndex(
                name: "IX_Validacoes_Identidade_Id_Tipo_Validacao",
                table: "Validacoes_Identidade",
                column: "Id_Tipo_Validacao");

            migrationBuilder.CreateIndex(
                name: "IX_Validacoes_Identidade_Id_Utilizador",
                table: "Validacoes_Identidade",
                column: "Id_Utilizador");

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Professor_Candidaturas_Id_Candidatura",
                table: "Avaliacoes_Professor",
                column: "Id_Candidatura",
                principalTable: "Candidaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Professor_Decisoes_Avaliacao_Id_Decisao_Avaliacao",
                table: "Avaliacoes_Professor",
                column: "Id_Decisao_Avaliacao",
                principalTable: "Decisoes_Avaliacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Avaliacoes_Professor_Professores_Id_Professor",
                table: "Avaliacoes_Professor",
                column: "Id_Professor",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_Estados_Candidatura_Id_Estado_Candidatura",
                table: "Candidaturas",
                column: "Id_Estado_Candidatura",
                principalTable: "Estados_Candidatura",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Estados_Oferta_Id_Estado_Oferta",
                table: "Ofertas",
                column: "Id_Estado_Oferta",
                principalTable: "Estados_Oferta",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Tipos_Oferta_Id_Tipo_Oferta",
                table: "Ofertas",
                column: "Id_Tipo_Oferta",
                principalTable: "Tipos_Oferta",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Estados_Validacao_Utilizador_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores",
                column: "Id_Estado_Validacao_Utilizador",
                principalTable: "Estados_Validacao_Utilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Tipos_Utilizador_Id_Tipo_Utilizador",
                table: "Utilizadores",
                column: "Id_Tipo_Utilizador",
                principalTable: "Tipos_Utilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Professor_Candidaturas_Id_Candidatura",
                table: "Avaliacoes_Professor");

            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Professor_Decisoes_Avaliacao_Id_Decisao_Avaliacao",
                table: "Avaliacoes_Professor");

            migrationBuilder.DropForeignKey(
                name: "FK_Avaliacoes_Professor_Professores_Id_Professor",
                table: "Avaliacoes_Professor");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Estados_Candidatura_Id_Estado_Candidatura",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Estados_Oferta_Id_Estado_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Tipos_Oferta_Id_Tipo_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Estados_Validacao_Utilizador_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Tipos_Utilizador_Id_Tipo_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Candidaturas_Ficheiros");

            migrationBuilder.DropTable(
                name: "Decisoes_Avaliacao");

            migrationBuilder.DropTable(
                name: "Utilizadores_Normais");

            migrationBuilder.DropTable(
                name: "Validacoes_Identidade");

            migrationBuilder.DropTable(
                name: "Estados_Validacao_Documento");

            migrationBuilder.DropTable(
                name: "Tipos_Validacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tipos_Utilizador",
                table: "Tipos_Utilizador");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tipos_Oferta",
                table: "Tipos_Oferta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estados_Validacao_Utilizador",
                table: "Estados_Validacao_Utilizador");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estados_Oferta",
                table: "Estados_Oferta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estados_Candidatura",
                table: "Estados_Candidatura");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Avaliacoes_Professor",
                table: "Avaliacoes_Professor");

            migrationBuilder.DropIndex(
                name: "IX_Avaliacoes_Professor_Id_Decisao_Avaliacao",
                table: "Avaliacoes_Professor");

            migrationBuilder.DeleteData(
                table: "Estados_Candidatura",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Estados_Oferta",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Tipos_Utilizador",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Id_Decisao_Avaliacao",
                table: "Avaliacoes_Professor");

            migrationBuilder.RenameTable(
                name: "Tipos_Utilizador",
                newName: "TipoUtilizadores");

            migrationBuilder.RenameTable(
                name: "Tipos_Oferta",
                newName: "TipoOfertas");

            migrationBuilder.RenameTable(
                name: "Estados_Validacao_Utilizador",
                newName: "EstadoValidacaoUtilizadores");

            migrationBuilder.RenameTable(
                name: "Estados_Oferta",
                newName: "EstadoOfertas");

            migrationBuilder.RenameTable(
                name: "Estados_Candidatura",
                newName: "EstadoCandidaturas");

            migrationBuilder.RenameTable(
                name: "Avaliacoes_Professor",
                newName: "AvaliacoesProfessores");

            migrationBuilder.RenameColumn(
                name: "Palavra_Passe",
                table: "Utilizadores",
                newName: "PalavraPasse");

            migrationBuilder.RenameColumn(
                name: "Data_Registro",
                table: "Utilizadores",
                newName: "DataRegisto");

            migrationBuilder.RenameColumn(
                name: "Data_Nascimento",
                table: "Utilizadores",
                newName: "DataNascimento");

            migrationBuilder.RenameColumn(
                name: "Numero_Professor",
                table: "Professores",
                newName: "NumeroProfessor");

            migrationBuilder.RenameColumn(
                name: "Data_Publicacao",
                table: "Ofertas",
                newName: "DataPublicacao");

            migrationBuilder.RenameColumn(
                name: "Data_Expiracao",
                table: "Ofertas",
                newName: "DataExpiracao");

            migrationBuilder.RenameColumn(
                name: "Nif",
                table: "Empresas",
                newName: "NIF");

            migrationBuilder.RenameColumn(
                name: "Site_Empresa",
                table: "Empresas",
                newName: "SiteEmpresa");

            migrationBuilder.RenameColumn(
                name: "Nome_Empresa",
                table: "Empresas",
                newName: "NomeEmpresa");

            migrationBuilder.RenameColumn(
                name: "Data_Candidatura",
                table: "Candidaturas",
                newName: "DataCandidatura");

            migrationBuilder.RenameColumn(
                name: "Numero_Aluno",
                table: "Alunos",
                newName: "NumeroAluno");

            migrationBuilder.RenameColumn(
                name: "Ano_Letivo",
                table: "Alunos",
                newName: "AnoLetivo");

            migrationBuilder.RenameColumn(
                name: "Data_Avaliacao",
                table: "AvaliacoesProfessores",
                newName: "DataAvaliacao");

            migrationBuilder.RenameIndex(
                name: "IX_Avaliacoes_Professor_Id_Professor",
                table: "AvaliacoesProfessores",
                newName: "IX_AvaliacoesProfessores_Id_Professor");

            migrationBuilder.RenameIndex(
                name: "IX_Avaliacoes_Professor_Id_Candidatura",
                table: "AvaliacoesProfessores",
                newName: "IX_AvaliacoesProfessores_Id_Candidatura");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoUtilizadores",
                table: "TipoUtilizadores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TipoOfertas",
                table: "TipoOfertas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadoValidacaoUtilizadores",
                table: "EstadoValidacaoUtilizadores",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadoOfertas",
                table: "EstadoOfertas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadoCandidaturas",
                table: "EstadoCandidaturas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvaliacoesProfessores",
                table: "AvaliacoesProfessores",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "TipoOfertas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Descricao",
                value: "Estágio curricular");

            migrationBuilder.UpdateData(
                table: "TipoOfertas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Descricao",
                value: "Projeto temporário");

            migrationBuilder.UpdateData(
                table: "TipoUtilizadores",
                keyColumn: "Id",
                keyValue: 4,
                column: "Designacao",
                value: "Administrador");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesProfessores_Candidaturas_Id_Candidatura",
                table: "AvaliacoesProfessores",
                column: "Id_Candidatura",
                principalTable: "Candidaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesProfessores_Professores_Id_Professor",
                table: "AvaliacoesProfessores",
                column: "Id_Professor",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_EstadoCandidaturas_Id_Estado_Candidatura",
                table: "Candidaturas",
                column: "Id_Estado_Candidatura",
                principalTable: "EstadoCandidaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_EstadoOfertas_Id_Estado_Oferta",
                table: "Ofertas",
                column: "Id_Estado_Oferta",
                principalTable: "EstadoOfertas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_TipoOfertas_Id_Tipo_Oferta",
                table: "Ofertas",
                column: "Id_Tipo_Oferta",
                principalTable: "TipoOfertas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_EstadoValidacaoUtilizadores_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores",
                column: "Id_Estado_Validacao_Utilizador",
                principalTable: "EstadoValidacaoUtilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_TipoUtilizadores_Id_Tipo_Utilizador",
                table: "Utilizadores",
                column: "Id_Tipo_Utilizador",
                principalTable: "TipoUtilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
