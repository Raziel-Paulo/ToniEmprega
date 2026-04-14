// Models/DocumentoValidacao.cs (NOVO)
namespace ToniEmprega.Models
{
    public class DocumentoValidacao
    {
        public int Id { get; set; }

        public int Id_Validacao_Identidade { get; set; }
        public ValidacaoIdentidade ValidacaoIdentidade { get; set; } = null!;

        public string Tipo_Documento { get; set; } = string.Empty; // CV, BI, CC, etc.
        public string Nome_Ficheiro { get; set; } = string.Empty;
        public string Caminho_Ficheiro { get; set; } = string.Empty;
        public DateTime Data_Upload { get; set; } = DateTime.Now;
    }
}