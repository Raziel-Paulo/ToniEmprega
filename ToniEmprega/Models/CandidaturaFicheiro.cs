// Models/CandidaturaFicheiro.cs (NOVO)
namespace ToniEmprega.Models
{
    public class CandidaturaFicheiro
    {
        public int Id { get; set; }

        public int Id_Candidatura { get; set; }
        public Candidatura Candidatura { get; set; } = null!;

        public string Tipo_Ficheiro { get; set; } = string.Empty; // CV, Carta Apresentação
        public string Nome_Ficheiro { get; set; } = string.Empty;
        public string Caminho_Ficheiro { get; set; } = string.Empty;
        public DateTime Data_Upload { get; set; } = DateTime.Now;
    }
}