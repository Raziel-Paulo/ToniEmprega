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
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DecisoesAvaliacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
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
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
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
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
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
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
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
                    Nome = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
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
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
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
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
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
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TiposValidacao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoUtilizadorId = table.Column<int>(type: "int", nullable: false),
                    EstadoValidacaoUtilizadorId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_EstadosValidacaoUtilizador_EstadoValidacaoUtilizadorId",
                        column: x => x.EstadoValidacaoUtilizadorId,
                        principalTable: "EstadosValidacaoUtilizador",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUsers_TiposUtilizador_TipoUtilizadorId",
                        column: x => x.TipoUtilizadorId,
                        principalTable: "TiposUtilizador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Requisitos = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TipoOfertaId = table.Column<int>(type: "int", nullable: false),
                    EstadoOfertaId = table.Column<int>(type: "int", nullable: false),
                    EmpresaId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpiracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ofertas_AspNetUsers_EmpresaId",
                        column: x => x.EmpresaId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ofertas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ofertas_EstadosOferta_EstadoOfertaId",
                        column: x => x.EstadoOfertaId,
                        principalTable: "EstadosOferta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ofertas_TiposOferta_TipoOfertaId",
                        column: x => x.TipoOfertaId,
                        principalTable: "TiposOferta",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValidacoesIdentidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilizadorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TipoValidacaoId = table.Column<int>(type: "int", nullable: false),
                    CaminhoDocumento = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    EstadoValidacaoUtilizadorId = table.Column<int>(type: "int", nullable: false),
                    EstadoValidacaoDocumentoId = table.Column<int>(type: "int", nullable: false),
                    DataSubmissao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidacoesIdentidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ValidacoesIdentidade_AspNetUsers_UtilizadorId",
                        column: x => x.UtilizadorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValidacoesIdentidade_EstadosValidacaoDocumento_EstadoValidacaoDocumentoId",
                        column: x => x.EstadoValidacaoDocumentoId,
                        principalTable: "EstadosValidacaoDocumento",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValidacoesIdentidade_EstadosValidacaoUtilizador_EstadoValidacaoUtilizadorId",
                        column: x => x.EstadoValidacaoUtilizadorId,
                        principalTable: "EstadosValidacaoUtilizador",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValidacoesIdentidade_TiposValidacao_TipoValidacaoId",
                        column: x => x.TipoValidacaoId,
                        principalTable: "TiposValidacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Candidaturas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfertaId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AlunoId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EstadoCandidaturaId = table.Column<int>(type: "int", nullable: false),
                    DataSubmissao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Candidaturas_AspNetUsers_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidaturas_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Candidaturas_EstadosCandidatura_EstadoCandidaturaId",
                        column: x => x.EstadoCandidaturaId,
                        principalTable: "EstadosCandidatura",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidaturas_Ofertas_OfertaId",
                        column: x => x.OfertaId,
                        principalTable: "Ofertas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AvaliacoesProfessor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidaturaId = table.Column<int>(type: "int", nullable: false),
                    ProfessorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DecisaoId = table.Column<int>(type: "int", nullable: false),
                    Comentario = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvaliacoesProfessor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvaliacoesProfessor_AspNetUsers_ProfessorId",
                        column: x => x.ProfessorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AvaliacoesProfessor_Candidaturas_CandidaturaId",
                        column: x => x.CandidaturaId,
                        principalTable: "Candidaturas",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AvaliacoesProfessor_DecisoesAvaliacao_DecisaoId",
                        column: x => x.DecisaoId,
                        principalTable: "DecisoesAvaliacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidaturasFicheiros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomeFicheiro = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Caminho = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CandidaturaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidaturasFicheiros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidaturasFicheiros_Candidaturas_CandidaturaId",
                        column: x => x.CandidaturaId,
                        principalTable: "Candidaturas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DecisoesAvaliacao",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Aprovado" },
                    { 2, "Rejeitado" },
                    { 3, "Rever" }
                });

            migrationBuilder.InsertData(
                table: "EstadosCandidatura",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Aceite" },
                    { 3, "Rejeitada" }
                });

            migrationBuilder.InsertData(
                table: "EstadosOferta",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Ativa" },
                    { 2, "Expirada" },
                    { 3, "Removida" }
                });

            migrationBuilder.InsertData(
                table: "EstadosValidacaoUtilizador",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Pendente" },
                    { 2, "Aprovado" },
                    { 3, "Rejeitado" }
                });

            migrationBuilder.InsertData(
                table: "TiposUtilizador",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Aluno" },
                    { 2, "Empresa" },
                    { 3, "Professor" },
                    { 4, "Basic" },
                    { 5, "Moderator" },
                    { 6, "Admin" },
                    { 7, "SuperAdmin" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EstadoValidacaoUtilizadorId",
                table: "AspNetUsers",
                column: "EstadoValidacaoUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TipoUtilizadorId",
                table: "AspNetUsers",
                column: "TipoUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessor_CandidaturaId",
                table: "AvaliacoesProfessor",
                column: "CandidaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessor_DecisaoId",
                table: "AvaliacoesProfessor",
                column: "DecisaoId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessor_ProfessorId",
                table: "AvaliacoesProfessor",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_AlunoId",
                table: "Candidaturas",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_EstadoCandidaturaId",
                table: "Candidaturas",
                column: "EstadoCandidaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_OfertaId",
                table: "Candidaturas",
                column: "OfertaId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_UserId",
                table: "Candidaturas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidaturasFicheiros_CandidaturaId",
                table: "CandidaturasFicheiros",
                column: "CandidaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_EmpresaId",
                table: "Ofertas",
                column: "EmpresaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_EstadoOfertaId",
                table: "Ofertas",
                column: "EstadoOfertaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_TipoOfertaId",
                table: "Ofertas",
                column: "TipoOfertaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_UserId",
                table: "Ofertas",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesIdentidade_EstadoValidacaoDocumentoId",
                table: "ValidacoesIdentidade",
                column: "EstadoValidacaoDocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesIdentidade_EstadoValidacaoUtilizadorId",
                table: "ValidacoesIdentidade",
                column: "EstadoValidacaoUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesIdentidade_TipoValidacaoId",
                table: "ValidacoesIdentidade",
                column: "TipoValidacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_ValidacoesIdentidade_UtilizadorId",
                table: "ValidacoesIdentidade",
                column: "UtilizadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AvaliacoesProfessor");

            migrationBuilder.DropTable(
                name: "CandidaturasFicheiros");

            migrationBuilder.DropTable(
                name: "ValidacoesIdentidade");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DecisoesAvaliacao");

            migrationBuilder.DropTable(
                name: "Candidaturas");

            migrationBuilder.DropTable(
                name: "EstadosValidacaoDocumento");

            migrationBuilder.DropTable(
                name: "TiposValidacao");

            migrationBuilder.DropTable(
                name: "EstadosCandidatura");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EstadosOferta");

            migrationBuilder.DropTable(
                name: "TiposOferta");

            migrationBuilder.DropTable(
                name: "EstadosValidacaoUtilizador");

            migrationBuilder.DropTable(
                name: "TiposUtilizador");
        }
    }
}
