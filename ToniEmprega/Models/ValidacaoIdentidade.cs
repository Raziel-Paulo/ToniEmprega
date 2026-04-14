// Models/ValidacaoIdentidade.cs (ATUALIZADO)
using System.ComponentModel.DataAnnotations.Schema;

namespace ToniEmprega.Models
{
    public class ValidacaoIdentidade
    {
        public int Id { get; set; }

        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;

        // ✅ MODIFICADO: Agora é uma coleção de documentos
        public ICollection<DocumentoValidacao> Documentos { get; set; } = new List<DocumentoValidacao>();

        public DateTime? Data_Validacao { get; set; }
        public DateTime Data_Criacao { get; set; } = DateTime.Now;

        public int? Id_Estado_Validacao_Documento { get; set; }
        public EstadoValidacaoDocumento? EstadoValidacaoDocumento { get; set; }

        // ✅ NOVO: Motivo da rejeição (para mostrar ao utilizador)
        public string? Motivo_Rejeicao { get; set; }
    }
}