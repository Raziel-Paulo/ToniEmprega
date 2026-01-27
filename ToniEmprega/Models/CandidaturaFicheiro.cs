using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CandidaturasFicheiros")]
public class CandidaturaFicheiro
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(150)]
    public string NomeFicheiro { get; set; }

    [Required, StringLength(250)]
    public string Caminho { get; set; }

    [Required, ForeignKey("Candidatura")]
    public int CandidaturaId { get; set; }
    public Candidatura Candidatura { get; set; }
}
