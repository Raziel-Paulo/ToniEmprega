using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("ValidacoesIdentidade")]
public class ValidacaoIdentidade
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UtilizadorId { get; set; }
    public ApplicationUser Utilizador { get; set; }

    [Required, ForeignKey("TipoValidacao")]
    public int TipoValidacaoId { get; set; }
    public TipoValidacao TipoValidacao { get; set; }

    [Required, StringLength(250)]
    public string CaminhoDocumento { get; set; }

    [Required, ForeignKey("EstadoUtilizador")]
    public int EstadoValidacaoUtilizadorId { get; set; }
    public EstadoValidacaoUtilizador EstadoUtilizador { get; set; }

    [Required, ForeignKey("EstadoDocumento")]
    public int EstadoValidacaoDocumentoId { get; set; }
    public EstadoValidacaoDocumento EstadoDocumento { get; set; }

    [Required]
    public DateTime DataSubmissao { get; set; }
}
