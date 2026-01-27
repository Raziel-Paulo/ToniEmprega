using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("TiposValidacao")]
public class TipoValidacao
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Nome { get; set; }
}
