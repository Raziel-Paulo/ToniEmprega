using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToniEmprega.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Utilizadores_UtilizadorId",
                table: "Alunos");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Candidaturas_CandidaturaId",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Professores_ProfessorId",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Alunos_AlunoId",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_EstadoCandidaturas_EstadoCandidaturaId",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Ofertas_OfertaId",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Utilizadores_UtilizadorId",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Empresas_EmpresaId",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_EstadoOfertas_EstadoOfertaId",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_TipoOfertas_TipoOfertaId",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Professores_Utilizadores_UtilizadorId",
                table: "Professores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_EstadoValidacaoUtilizadores_EstadoValidacaoId",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_TipoUtilizadores_TipoUtilizadorId",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_EstadoValidacaoId",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_TipoUtilizadorId",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Professores_UtilizadorId",
                table: "Professores");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_EmpresaId",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_EstadoOfertaId",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_TipoOfertaId",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_UtilizadorId",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Candidaturas_AlunoId",
                table: "Candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_Candidaturas_EstadoCandidaturaId",
                table: "Candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_Candidaturas_OfertaId",
                table: "Candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesProfessores_CandidaturaId",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesProfessores_ProfessorId",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_UtilizadorId",
                table: "Alunos");

            migrationBuilder.DropColumn(
                name: "EstadoValidacaoId",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "TipoUtilizadorId",
                table: "Utilizadores");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "Professores");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Ofertas");

            migrationBuilder.DropColumn(
                name: "EstadoOfertaId",
                table: "Ofertas");

            migrationBuilder.DropColumn(
                name: "TipoOfertaId",
                table: "Ofertas");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "Empresas");

            migrationBuilder.DropColumn(
                name: "AlunoId",
                table: "Candidaturas");

            migrationBuilder.DropColumn(
                name: "EstadoCandidaturaId",
                table: "Candidaturas");

            migrationBuilder.DropColumn(
                name: "OfertaId",
                table: "Candidaturas");

            migrationBuilder.DropColumn(
                name: "CandidaturaId",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropColumn(
                name: "ProfessorId",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropColumn(
                name: "UtilizadorId",
                table: "Alunos");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores",
                column: "Id_Estado_Validacao_Utilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_Id_Tipo_Utilizador",
                table: "Utilizadores",
                column: "Id_Tipo_Utilizador");

            migrationBuilder.CreateIndex(
                name: "IX_Professores_Id_Utilizador",
                table: "Professores",
                column: "Id_Utilizador",
                unique: true);

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
                name: "IX_Empresas_Id_Utilizador",
                table: "Empresas",
                column: "Id_Utilizador",
                unique: true);

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
                name: "IX_AvaliacoesProfessores_Id_Candidatura",
                table: "AvaliacoesProfessores",
                column: "Id_Candidatura");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessores_Id_Professor",
                table: "AvaliacoesProfessores",
                column: "Id_Professor");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_Id_Utilizador",
                table: "Alunos",
                column: "Id_Utilizador",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Utilizadores_Id_Utilizador",
                table: "Alunos",
                column: "Id_Utilizador",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Candidaturas_Alunos_Id_Aluno",
                table: "Candidaturas",
                column: "Id_Aluno",
                principalTable: "Alunos",
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
                name: "FK_Candidaturas_Ofertas_Id_Oferta",
                table: "Candidaturas",
                column: "Id_Oferta",
                principalTable: "Ofertas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Utilizadores_Id_Utilizador",
                table: "Empresas",
                column: "Id_Utilizador",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Empresas_Id_Empresa",
                table: "Ofertas",
                column: "Id_Empresa",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Professores_Utilizadores_Id_Utilizador",
                table: "Professores",
                column: "Id_Utilizador",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alunos_Utilizadores_Id_Utilizador",
                table: "Alunos");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Candidaturas_Id_Candidatura",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Professores_Id_Professor",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Alunos_Id_Aluno",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_EstadoCandidaturas_Id_Estado_Candidatura",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_Ofertas_Id_Oferta",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresas_Utilizadores_Id_Utilizador",
                table: "Empresas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_Empresas_Id_Empresa",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_EstadoOfertas_Id_Estado_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_TipoOfertas_Id_Tipo_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Professores_Utilizadores_Id_Utilizador",
                table: "Professores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_EstadoValidacaoUtilizadores_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_TipoUtilizadores_Id_Tipo_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Utilizadores_Id_Tipo_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropIndex(
                name: "IX_Professores_Id_Utilizador",
                table: "Professores");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_Id_Empresa",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_Id_Estado_Oferta",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Ofertas_Id_Tipo_Oferta",
                table: "Ofertas");

            migrationBuilder.DropIndex(
                name: "IX_Empresas_Id_Utilizador",
                table: "Empresas");

            migrationBuilder.DropIndex(
                name: "IX_Candidaturas_Id_Aluno",
                table: "Candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_Candidaturas_Id_Estado_Candidatura",
                table: "Candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_Candidaturas_Id_Oferta",
                table: "Candidaturas");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesProfessores_Id_Candidatura",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropIndex(
                name: "IX_AvaliacoesProfessores_Id_Professor",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropIndex(
                name: "IX_Alunos_Id_Utilizador",
                table: "Alunos");

            migrationBuilder.AddColumn<int>(
                name: "EstadoValidacaoId",
                table: "Utilizadores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoUtilizadorId",
                table: "Utilizadores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UtilizadorId",
                table: "Professores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Ofertas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstadoOfertaId",
                table: "Ofertas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoOfertaId",
                table: "Ofertas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UtilizadorId",
                table: "Empresas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AlunoId",
                table: "Candidaturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstadoCandidaturaId",
                table: "Candidaturas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfertaId",
                table: "Candidaturas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CandidaturaId",
                table: "AvaliacoesProfessores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId",
                table: "AvaliacoesProfessores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UtilizadorId",
                table: "Alunos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_EstadoValidacaoId",
                table: "Utilizadores",
                column: "EstadoValidacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Utilizadores_TipoUtilizadorId",
                table: "Utilizadores",
                column: "TipoUtilizadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Professores_UtilizadorId",
                table: "Professores",
                column: "UtilizadorId");

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
                name: "IX_Empresas_UtilizadorId",
                table: "Empresas",
                column: "UtilizadorId");

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
                name: "IX_AvaliacoesProfessores_CandidaturaId",
                table: "AvaliacoesProfessores",
                column: "CandidaturaId");

            migrationBuilder.CreateIndex(
                name: "IX_AvaliacoesProfessores_ProfessorId",
                table: "AvaliacoesProfessores",
                column: "ProfessorId");

            migrationBuilder.CreateIndex(
                name: "IX_Alunos_UtilizadorId",
                table: "Alunos",
                column: "UtilizadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alunos_Utilizadores_UtilizadorId",
                table: "Alunos",
                column: "UtilizadorId",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesProfessores_Candidaturas_CandidaturaId",
                table: "AvaliacoesProfessores",
                column: "CandidaturaId",
                principalTable: "Candidaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesProfessores_Professores_ProfessorId",
                table: "AvaliacoesProfessores",
                column: "ProfessorId",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_Alunos_AlunoId",
                table: "Candidaturas",
                column: "AlunoId",
                principalTable: "Alunos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_EstadoCandidaturas_EstadoCandidaturaId",
                table: "Candidaturas",
                column: "EstadoCandidaturaId",
                principalTable: "EstadoCandidaturas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_Ofertas_OfertaId",
                table: "Candidaturas",
                column: "OfertaId",
                principalTable: "Ofertas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresas_Utilizadores_UtilizadorId",
                table: "Empresas",
                column: "UtilizadorId",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_Empresas_EmpresaId",
                table: "Ofertas",
                column: "EmpresaId",
                principalTable: "Empresas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_EstadoOfertas_EstadoOfertaId",
                table: "Ofertas",
                column: "EstadoOfertaId",
                principalTable: "EstadoOfertas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_TipoOfertas_TipoOfertaId",
                table: "Ofertas",
                column: "TipoOfertaId",
                principalTable: "TipoOfertas",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Professores_Utilizadores_UtilizadorId",
                table: "Professores",
                column: "UtilizadorId",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_EstadoValidacaoUtilizadores_EstadoValidacaoId",
                table: "Utilizadores",
                column: "EstadoValidacaoId",
                principalTable: "EstadoValidacaoUtilizadores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_TipoUtilizadores_TipoUtilizadorId",
                table: "Utilizadores",
                column: "TipoUtilizadorId",
                principalTable: "TipoUtilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
