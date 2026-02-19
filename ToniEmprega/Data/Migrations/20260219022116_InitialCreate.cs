using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToniEmprega.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DecisoesAvaliacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DecisoesAvaliacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosCandidatura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosCandidatura", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosOferta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosOferta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosValidacaoDocumento",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosValidacaoDocumento", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EstadosValidacaoUtilizador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosValidacaoUtilizador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposOferta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposOferta", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposUtilizador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposUtilizador", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TiposValidacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Designacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposValidacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilizadores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Palavra_Passe = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Nascimento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data_Registro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_Tipo_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Id_Estado_Validacao_Utilizador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilizadores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Utilizadores_EstadosValidacaoUtilizador_Id_Estado_Validacao_Utilizador",
                        column: x => x.Id_Estado_Validacao_Utilizador,
                        principalTable: "EstadosValidacaoUtilizador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Utilizadores_TiposUtilizador_Id_Tipo_Utilizador",
                        column: x => x.Id_Tipo_Utilizador,
                        principalTable: "TiposUtilizador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Curso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ano_Letivo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero_Aluno = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alunos_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Nome_Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Site_Empresa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Empresas_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lida = table.Column<bool>(type: "bit", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notificacoes_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Professores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Departamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero_Professor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professores_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UtilizadoresNormais",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Utilizador = table.Column<int>(type: "int", nullable: false),
                    Documentacao_Identificacao = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilizadoresNormais", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilizadoresNormais_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidacoesIdentidade",
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
                    table.PrimaryKey("PK_ValidacoesIdentidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidacoesIdentidade_EstadosValidacaoDocumento_Id_Estado_Validacao_Documento",
                        column: x => x.Id_Estado_Validacao_Documento,
                        principalTable: "EstadosValidacaoDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ValidacoesIdentidade_TiposValidacao_Id_Tipo_Validacao",
                        column: x => x.Id_Tipo_Validacao,
                        principalTable: "TiposValidacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValidacoesIdentidade_Utilizadores_Id_Utilizador",
                        column: x => x.Id_Utilizador,
                        principalTable: "Utilizadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Empresa = table.Column<int>(type: "int", nullable: false),
                    Id_Tipo_Oferta = table.Column<int>(type: "int", nullable: true),
                    Id_Estado_Oferta = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requisitos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Publicacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data_Expiracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ofertas_Empresas_Id_Empresa",
                        column: x => x.Id_Empresa,
                        principalTable: "Empresas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ofertas_EstadosOferta_Id_Estado_Oferta",
                        column: x => x.Id_Estado_Oferta,
                        principalTable: "EstadosOferta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Ofertas_TiposOferta_Id_Tipo_Oferta",
                        column: x => x.Id_Tipo_Oferta,
                        principalTable: "TiposOferta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Candidaturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Oferta = table.Column<int>(type: "int", nullable: false),
                    Id_Aluno = table.Column<int>(type: "int", nullable: false),
                    Id_Estado_Candidatura = table.Column<int>(type: "int", nullable: true),
                    Data_Candidatura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidaturas_Alunos_Id_Aluno",
                        column: x => x.Id_Aluno,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Candidaturas_EstadosCandidatura_Id_Estado_Candidatura",
                        column: x => x.Id_Estado_Candidatura,
                        principalTable: "EstadosCandidatura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Candidaturas_Ofertas_Id_Oferta",
                        column: x => x.Id_Oferta,
                        principalTable: "Ofertas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AvaliacoesProfessores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Candidatura = table.Column<int>(type: "int", nullable: false),
                    Id_Professor = table.Column<int>(type: "int", nullable: false),
                    Id_Decisao_Avaliacao = table.Column<int>(type: "int", nullable: true),
                    Comentarios = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data_Avaliacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacoesProfessores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacoesProfessores_Candidaturas_Id_Candidatura",
                        column: x => x.Id_Candidatura,
                        principalTable: "Candidaturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvaliacoesProfessores_DecisoesAvaliacao_Id_Decisao_Avaliacao",
                        column: x => x.Id_Decisao_Avaliacao,
                        principalTable: "DecisoesAvaliacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_AvaliacoesProfessores_Professores_Id_Professor",
                        column: x => x.Id_Professor,
                        principalTable: "Professores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CandidaturasFicheiros",
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
                    table.PrimaryKey("PK_CandidaturasFicheiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidaturasFicheiros_Candidaturas_Id_Candidatura",
                        column: x => x.Id_Candidatura,
                        principalTable: "Candidaturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DecisoesAvaliacao",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Aprovado" },
                    { 2, "Rejeitado" },
                    { 3, "Necessita de Revisão" }
                });

            migrationBuilder.InsertData(
                table: "EstadosCandidatura",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Em Análise" },
                    { 3, "Aprovada" },
                    { 4, "Rejeitada" },
                    { 5, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "EstadosOferta",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Ativa" },
                    { 2, "Expirada" },
                    { 3, "Preenchida" },
                    { 4, "Desativada" }
                });

            migrationBuilder.InsertData(
                table: "EstadosValidacaoDocumento",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Aprovado" },
                    { 3, "Rejeitado" }
                });

            migrationBuilder.InsertData(
                table: "EstadosValidacaoUtilizador",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Aprovado" },
                    { 3, "Rejeitado" }
                });

            migrationBuilder.InsertData(
                table: "TiposOferta",
                columns: new[] { "Id", "Descricao", "Designacao" },
                values: new object[,]
                {
                    { 1, "Estágio curricular ou profissional", "Estágio" },
                    { 2, "Contrato de trabalho", "Emprego" },
                    { 3, "Participação em projeto", "Projeto" }
                });

            migrationBuilder.InsertData(
                table: "TiposUtilizador",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Aluno" },
                    { 2, "Professor" },
                    { 3, "Empresa" },
                    { 4, "Utilizador Normal" },
                    { 5, "Administrador" }
                });

            migrationBuilder.InsertData(
                table: "TiposValidacao",
                columns: new[] { "Id", "Designacao" },
                values: new object[,]
                {
                    { 1, "Cartão de Estudante" },
                    { 2, "Bilhete de Identidade" },
                    { 3, "Cartão de Cidadão" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Id_Utilizador",
                table: "Admins",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Id_Utilizador",
                table: "Alunos",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessores_Id_Candidatura",
                table: "AvaliacoesProfessores",
                column: "Id_Candidatura");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessores_Id_Decisao_Avaliacao",
                table: "AvaliacoesProfessores",
                column: "Id_Decisao_Avaliacao");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessores_Id_Professor",
                table: "AvaliacoesProfessores",
                column: "Id_Professor");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_Id_Aluno",
                table: "Candidaturas",
                column: "Id_Aluno");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_Id_Estado_Candidatura",
                table: "Candidaturas",
                column: "Id_Estado_Candidatura");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_Id_Oferta",
                table: "Candidaturas",
                column: "Id_Oferta");

            migrationBuilder.CreateIndex(
                name: "IX_CandidaturasFicheiros_Id_Candidatura",
                table: "CandidaturasFicheiros",
                column: "Id_Candidatura");

            migrationBuilder.CreateIndex(
                name: "IX_Empresas_Id_Utilizador",
                table: "Empresas",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notificacoes_Id_Utilizador",
                table: "Notificacoes",
                column: "Id_Utilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_Id_Empresa",
                table: "Ofertas",
                column: "Id_Empresa");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_Id_Estado_Oferta",
                table: "Ofertas",
                column: "Id_Estado_Oferta");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_Id_Tipo_Oferta",
                table: "Ofertas",
                column: "Id_Tipo_Oferta");

            migrationBuilder.CreateIndex(
                name: "IX_Professores_Id_Utilizador",
                table: "Professores",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores",
                column: "Id_Estado_Validacao_Utilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_Id_Tipo_Utilizador",
                table: "Utilizadores",
                column: "Id_Tipo_Utilizador");

            migrationBuilder.CreateIndex(
                name: "IX_UtilizadoresNormais_Id_Utilizador",
                table: "UtilizadoresNormais",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesIdentidade_Id_Estado_Validacao_Documento",
                table: "ValidacoesIdentidade",
                column: "Id_Estado_Validacao_Documento");

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesIdentidade_Id_Tipo_Validacao",
                table: "ValidacoesIdentidade",
                column: "Id_Tipo_Validacao");

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesIdentidade_Id_Utilizador",
                table: "ValidacoesIdentidade",
                column: "Id_Utilizador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "AvaliacoesProfessores");

            migrationBuilder.DropTable(
                name: "CandidaturasFicheiros");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "UtilizadoresNormais");

            migrationBuilder.DropTable(
                name: "ValidacoesIdentidade");

            migrationBuilder.DropTable(
                name: "DecisoesAvaliacao");

            migrationBuilder.DropTable(
                name: "Professores");

            migrationBuilder.DropTable(
                name: "Candidaturas");

            migrationBuilder.DropTable(
                name: "EstadosValidacaoDocumento");

            migrationBuilder.DropTable(
                name: "TiposValidacao");

            migrationBuilder.DropTable(
                name: "Alunos");

            migrationBuilder.DropTable(
                name: "EstadosCandidatura");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "Empresas");

            migrationBuilder.DropTable(
                name: "EstadosOferta");

            migrationBuilder.DropTable(
                name: "TiposOferta");

            migrationBuilder.DropTable(
                name: "Utilizadores");

            migrationBuilder.DropTable(
                name: "EstadosValidacaoUtilizador");

            migrationBuilder.DropTable(
                name: "TiposUtilizador");
        }
    }
}
