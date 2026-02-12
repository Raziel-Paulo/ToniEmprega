// Models/ValidacaoIdentidade.cs (ATUALIZADO)
using System.ComponentModel.DataAnnotations.Schema;

namespace ToniEmprega.Models
{
    public class ValidacaoIdentidade
    {
        public int Id { get; set; }

        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;

        public int Id_Tipo_Validacao { get; set; }
        public TipoValidacao TipoValidacao { get; set; } = null!;

        public string Ficheiro_Prova { get; set; } = string.Empty; // Caminho do ficheiro
        public DateTime? Data_Validacao { get; set; }

        public int? Id_Estado_Validacao_Documento { get; set; }
        public EstadoValidacaoDocumento? EstadoValidacaoDocumento { get; set; }
    }
}