using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("EstadosOferta")]
public class EstadoOferta
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Nome { get; set; }
}
