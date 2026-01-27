using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToniEmprega.Models
{
    [Table("TiposUtilizador")]
    public class TipoUtilizador
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }
    }
}
