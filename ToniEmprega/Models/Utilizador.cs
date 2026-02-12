// Models/Utilizador.cs
using System.ComponentModel.DataAnnotations;

namespace ToniEmprega.Models
{
    public class Utilizador
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PalavraPasse { get; set; } = string.Empty;

        public DateTime? DataNascimento { get; set; }
        public DateTime DataRegisto { get; set; } = DateTime.Now;

        public int Id_Tipo_Utilizador { get; set; }
        public TipoUtilizador TipoUtilizador { get; set; } = null!;

        public int? Id_Estado_Validacao_Utilizador { get; set; }
        public EstadoValidacaoUtilizador? EstadoValidacao { get; set; }
    }
}