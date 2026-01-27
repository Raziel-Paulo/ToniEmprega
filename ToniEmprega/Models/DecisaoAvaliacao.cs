using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ToniEmprega.Models
{
    [Table("DecisoesAvaliacao")]
    public class DecisaoAvaliacao
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Nome { get; set; }
    }
}
