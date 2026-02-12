// Models/Utilizador.cs (ATUALIZADO)
namespace ToniEmprega.Models
{
    public class Utilizador
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Palavra_Passe { get; set; } = string.Empty;
        public DateTime? Data_Nascimento { get; set; }
        public DateTime Data_Registro { get; set; } = DateTime.Now;

        public int Id_Tipo_Utilizador { get; set; }
        public TipoUtilizador TipoUtilizador { get; set; } = null!;

        public int? Id_Estado_Validacao_Utilizador { get; set; }
        public EstadoValidacaoUtilizador? EstadoValidacao { get; set; }

        // Navegação
        public ICollection<ValidacaoIdentidade> ValidacoesIdentidade { get; set; } = new List<ValidacaoIdentidade>();
    }
}