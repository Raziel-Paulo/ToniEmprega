using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToniEmprega.Data.Migrations
{
    /// <inheritdoc />
    public partial class Bora : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_Candidaturas_Ficheiros_Candidaturas_Id_Candidatura",
                table: "Candidaturas_Ficheiros");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_Normais_Utilizadores_Id_Utilizador",
                table: "Utilizadores_Normais");

            migrationBuilder.DropForeignKey(
                name: "FK_Validacoes_Identidade_Estados_Validacao_Documento_Id_Estado_Validacao_Documento",
                table: "Validacoes_Identidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Validacoes_Identidade_Tipos_Validacao_Id_Tipo_Validacao",
                table: "Validacoes_Identidade");

            migrationBuilder.DropForeignKey(
                name: "FK_Validacoes_Identidade_Utilizadores_Id_Utilizador",
                table: "Validacoes_Identidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Validacoes_Identidade",
                table: "Validacoes_Identidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Utilizadores_Normais",
                table: "Utilizadores_Normais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tipos_Validacao",
                table: "Tipos_Validacao");

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
                name: "PK_Estados_Validacao_Documento",
                table: "Estados_Validacao_Documento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estados_Oferta",
                table: "Estados_Oferta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Estados_Candidatura",
                table: "Estados_Candidatura");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decisoes_Avaliacao",
                table: "Decisoes_Avaliacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Candidaturas_Ficheiros",
                table: "Candidaturas_Ficheiros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Avaliacoes_Professor",
                table: "Avaliacoes_Professor");

            migrationBuilder.RenameTable(
                name: "Validacoes_Identidade",
                newName: "ValidacoesIdentidade");

            migrationBuilder.RenameTable(
                name: "Utilizadores_Normais",
                newName: "UtilizadoresNormais");

            migrationBuilder.RenameTable(
                name: "Tipos_Validacao",
                newName: "TiposValidacao");

            migrationBuilder.RenameTable(
                name: "Tipos_Utilizador",
                newName: "TiposUtilizador");

            migrationBuilder.RenameTable(
                name: "Tipos_Oferta",
                newName: "TiposOferta");

            migrationBuilder.RenameTable(
                name: "Estados_Validacao_Utilizador",
                newName: "EstadosValidacaoUtilizador");

            migrationBuilder.RenameTable(
                name: "Estados_Validacao_Documento",
                newName: "EstadosValidacaoDocumento");

            migrationBuilder.RenameTable(
                name: "Estados_Oferta",
                newName: "EstadosOferta");

            migrationBuilder.RenameTable(
                name: "Estados_Candidatura",
                newName: "EstadosCandidatura");

            migrationBuilder.RenameTable(
                name: "Decisoes_Avaliacao",
                newName: "DecisoesAvaliacao");

            migrationBuilder.RenameTable(
                name: "Candidaturas_Ficheiros",
                newName: "CandidaturasFicheiros");

            migrationBuilder.RenameTable(
                name: "Avaliacoes_Professor",
                newName: "AvaliacoesProfessores");

            migrationBuilder.RenameIndex(
                name: "IX_Validacoes_Identidade_Id_Utilizador",
                table: "ValidacoesIdentidade",
                newName: "IX_ValidacoesIdentidade_Id_Utilizador");

            migrationBuilder.RenameIndex(
                name: "IX_Validacoes_Identidade_Id_Tipo_Validacao",
                table: "ValidacoesIdentidade",
                newName: "IX_ValidacoesIdentidade_Id_Tipo_Validacao");

            migrationBuilder.RenameIndex(
                name: "IX_Validacoes_Identidade_Id_Estado_Validacao_Documento",
                table: "ValidacoesIdentidade",
                newName: "IX_ValidacoesIdentidade_Id_Estado_Validacao_Documento");

            migrationBuilder.RenameIndex(
                name: "IX_Utilizadores_Normais_Id_Utilizador",
                table: "UtilizadoresNormais",
                newName: "IX_UtilizadoresNormais_Id_Utilizador");

            migrationBuilder.RenameIndex(
                name: "IX_Candidaturas_Ficheiros_Id_Candidatura",
                table: "CandidaturasFicheiros",
                newName: "IX_CandidaturasFicheiros_Id_Candidatura");

            migrationBuilder.RenameIndex(
                name: "IX_Avaliacoes_Professor_Id_Professor",
                table: "AvaliacoesProfessores",
                newName: "IX_AvaliacoesProfessores_Id_Professor");

            migrationBuilder.RenameIndex(
                name: "IX_Avaliacoes_Professor_Id_Decisao_Avaliacao",
                table: "AvaliacoesProfessores",
                newName: "IX_AvaliacoesProfessores_Id_Decisao_Avaliacao");

            migrationBuilder.RenameIndex(
                name: "IX_Avaliacoes_Professor_Id_Candidatura",
                table: "AvaliacoesProfessores",
                newName: "IX_AvaliacoesProfessores_Id_Candidatura");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ValidacoesIdentidade",
                table: "ValidacoesIdentidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UtilizadoresNormais",
                table: "UtilizadoresNormais",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TiposValidacao",
                table: "TiposValidacao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TiposUtilizador",
                table: "TiposUtilizador",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TiposOferta",
                table: "TiposOferta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadosValidacaoUtilizador",
                table: "EstadosValidacaoUtilizador",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadosValidacaoDocumento",
                table: "EstadosValidacaoDocumento",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadosOferta",
                table: "EstadosOferta",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EstadosCandidatura",
                table: "EstadosCandidatura",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DecisoesAvaliacao",
                table: "DecisoesAvaliacao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CandidaturasFicheiros",
                table: "CandidaturasFicheiros",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AvaliacoesProfessores",
                table: "AvaliacoesProfessores",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesProfessores_Candidaturas_Id_Candidatura",
                table: "AvaliacoesProfessores",
                column: "Id_Candidatura",
                principalTable: "Candidaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesProfessores_DecisoesAvaliacao_Id_Decisao_Avaliacao",
                table: "AvaliacoesProfessores",
                column: "Id_Decisao_Avaliacao",
                principalTable: "DecisoesAvaliacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_AvaliacoesProfessores_Professores_Id_Professor",
                table: "AvaliacoesProfessores",
                column: "Id_Professor",
                principalTable: "Professores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Candidaturas_EstadosCandidatura_Id_Estado_Candidatura",
                table: "Candidaturas",
                column: "Id_Estado_Candidatura",
                principalTable: "EstadosCandidatura",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidaturasFicheiros_Candidaturas_Id_Candidatura",
                table: "CandidaturasFicheiros",
                column: "Id_Candidatura",
                principalTable: "Candidaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_EstadosOferta_Id_Estado_Oferta",
                table: "Ofertas",
                column: "Id_Estado_Oferta",
                principalTable: "EstadosOferta",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Ofertas_TiposOferta_Id_Tipo_Oferta",
                table: "Ofertas",
                column: "Id_Tipo_Oferta",
                principalTable: "TiposOferta",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_EstadosValidacaoUtilizador_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores",
                column: "Id_Estado_Validacao_Utilizador",
                principalTable: "EstadosValidacaoUtilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_TiposUtilizador_Id_Tipo_Utilizador",
                table: "Utilizadores",
                column: "Id_Tipo_Utilizador",
                principalTable: "TiposUtilizador",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UtilizadoresNormais_Utilizadores_Id_Utilizador",
                table: "UtilizadoresNormais",
                column: "Id_Utilizador",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidacoesIdentidade_EstadosValidacaoDocumento_Id_Estado_Validacao_Documento",
                table: "ValidacoesIdentidade",
                column: "Id_Estado_Validacao_Documento",
                principalTable: "EstadosValidacaoDocumento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidacoesIdentidade_TiposValidacao_Id_Tipo_Validacao",
                table: "ValidacoesIdentidade",
                column: "Id_Tipo_Validacao",
                principalTable: "TiposValidacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValidacoesIdentidade_Utilizadores_Id_Utilizador",
                table: "ValidacoesIdentidade",
                column: "Id_Utilizador",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Candidaturas_Id_Candidatura",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_DecisoesAvaliacao_Id_Decisao_Avaliacao",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_AvaliacoesProfessores_Professores_Id_Professor",
                table: "AvaliacoesProfessores");

            migrationBuilder.DropForeignKey(
                name: "FK_Candidaturas_EstadosCandidatura_Id_Estado_Candidatura",
                table: "Candidaturas");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidaturasFicheiros_Candidaturas_Id_Candidatura",
                table: "CandidaturasFicheiros");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_EstadosOferta_Id_Estado_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Ofertas_TiposOferta_Id_Tipo_Oferta",
                table: "Ofertas");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_EstadosValidacaoUtilizador_Id_Estado_Validacao_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_Utilizadores_TiposUtilizador_Id_Tipo_Utilizador",
                table: "Utilizadores");

            migrationBuilder.DropForeignKey(
                name: "FK_UtilizadoresNormais_Utilizadores_Id_Utilizador",
                table: "UtilizadoresNormais");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidacoesIdentidade_EstadosValidacaoDocumento_Id_Estado_Validacao_Documento",
                table: "ValidacoesIdentidade");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidacoesIdentidade_TiposValidacao_Id_Tipo_Validacao",
                table: "ValidacoesIdentidade");

            migrationBuilder.DropForeignKey(
                name: "FK_ValidacoesIdentidade_Utilizadores_Id_Utilizador",
                table: "ValidacoesIdentidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ValidacoesIdentidade",
                table: "ValidacoesIdentidade");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UtilizadoresNormais",
                table: "UtilizadoresNormais");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TiposValidacao",
                table: "TiposValidacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TiposUtilizador",
                table: "TiposUtilizador");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TiposOferta",
                table: "TiposOferta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadosValidacaoUtilizador",
                table: "EstadosValidacaoUtilizador");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadosValidacaoDocumento",
                table: "EstadosValidacaoDocumento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadosOferta",
                table: "EstadosOferta");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EstadosCandidatura",
                table: "EstadosCandidatura");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DecisoesAvaliacao",
                table: "DecisoesAvaliacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CandidaturasFicheiros",
                table: "CandidaturasFicheiros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AvaliacoesProfessores",
                table: "AvaliacoesProfessores");

            migrationBuilder.RenameTable(
                name: "ValidacoesIdentidade",
                newName: "Validacoes_Identidade");

            migrationBuilder.RenameTable(
                name: "UtilizadoresNormais",
                newName: "Utilizadores_Normais");

            migrationBuilder.RenameTable(
                name: "TiposValidacao",
                newName: "Tipos_Validacao");

            migrationBuilder.RenameTable(
                name: "TiposUtilizador",
                newName: "Tipos_Utilizador");

            migrationBuilder.RenameTable(
                name: "TiposOferta",
                newName: "Tipos_Oferta");

            migrationBuilder.RenameTable(
                name: "EstadosValidacaoUtilizador",
                newName: "Estados_Validacao_Utilizador");

            migrationBuilder.RenameTable(
                name: "EstadosValidacaoDocumento",
                newName: "Estados_Validacao_Documento");

            migrationBuilder.RenameTable(
                name: "EstadosOferta",
                newName: "Estados_Oferta");

            migrationBuilder.RenameTable(
                name: "EstadosCandidatura",
                newName: "Estados_Candidatura");

            migrationBuilder.RenameTable(
                name: "DecisoesAvaliacao",
                newName: "Decisoes_Avaliacao");

            migrationBuilder.RenameTable(
                name: "CandidaturasFicheiros",
                newName: "Candidaturas_Ficheiros");

            migrationBuilder.RenameTable(
                name: "AvaliacoesProfessores",
                newName: "Avaliacoes_Professor");

            migrationBuilder.RenameIndex(
                name: "IX_ValidacoesIdentidade_Id_Utilizador",
                table: "Validacoes_Identidade",
                newName: "IX_Validacoes_Identidade_Id_Utilizador");

            migrationBuilder.RenameIndex(
                name: "IX_ValidacoesIdentidade_Id_Tipo_Validacao",
                table: "Validacoes_Identidade",
                newName: "IX_Validacoes_Identidade_Id_Tipo_Validacao");

            migrationBuilder.RenameIndex(
                name: "IX_ValidacoesIdentidade_Id_Estado_Validacao_Documento",
                table: "Validacoes_Identidade",
                newName: "IX_Validacoes_Identidade_Id_Estado_Validacao_Documento");

            migrationBuilder.RenameIndex(
                name: "IX_UtilizadoresNormais_Id_Utilizador",
                table: "Utilizadores_Normais",
                newName: "IX_Utilizadores_Normais_Id_Utilizador");

            migrationBuilder.RenameIndex(
                name: "IX_CandidaturasFicheiros_Id_Candidatura",
                table: "Candidaturas_Ficheiros",
                newName: "IX_Candidaturas_Ficheiros_Id_Candidatura");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesProfessores_Id_Professor",
                table: "Avaliacoes_Professor",
                newName: "IX_Avaliacoes_Professor_Id_Professor");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesProfessores_Id_Decisao_Avaliacao",
                table: "Avaliacoes_Professor",
                newName: "IX_Avaliacoes_Professor_Id_Decisao_Avaliacao");

            migrationBuilder.RenameIndex(
                name: "IX_AvaliacoesProfessores_Id_Candidatura",
                table: "Avaliacoes_Professor",
                newName: "IX_Avaliacoes_Professor_Id_Candidatura");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Validacoes_Identidade",
                table: "Validacoes_Identidade",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Utilizadores_Normais",
                table: "Utilizadores_Normais",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tipos_Validacao",
                table: "Tipos_Validacao",
                column: "Id");

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
                name: "PK_Estados_Validacao_Documento",
                table: "Estados_Validacao_Documento",
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
                name: "PK_Decisoes_Avaliacao",
                table: "Decisoes_Avaliacao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Candidaturas_Ficheiros",
                table: "Candidaturas_Ficheiros",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Avaliacoes_Professor",
                table: "Avaliacoes_Professor",
                column: "Id");

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
                name: "FK_Candidaturas_Ficheiros_Candidaturas_Id_Candidatura",
                table: "Candidaturas_Ficheiros",
                column: "Id_Candidatura",
                principalTable: "Candidaturas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Utilizadores_Normais_Utilizadores_Id_Utilizador",
                table: "Utilizadores_Normais",
                column: "Id_Utilizador",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Validacoes_Identidade_Estados_Validacao_Documento_Id_Estado_Validacao_Documento",
                table: "Validacoes_Identidade",
                column: "Id_Estado_Validacao_Documento",
                principalTable: "Estados_Validacao_Documento",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Validacoes_Identidade_Tipos_Validacao_Id_Tipo_Validacao",
                table: "Validacoes_Identidade",
                column: "Id_Tipo_Validacao",
                principalTable: "Tipos_Validacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Validacoes_Identidade_Utilizadores_Id_Utilizador",
                table: "Validacoes_Identidade",
                column: "Id_Utilizador",
                principalTable: "Utilizadores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
