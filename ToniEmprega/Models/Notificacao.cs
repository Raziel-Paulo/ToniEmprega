namespace ToniEmprega.Models
{
    public class Notificacao
    {
        public int Id { get; set; }
        public int Id_Utilizador { get; set; }
        public Utilizador Utilizador { get; set; } = null!;

        public string Titulo { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
        public DateTime Data_Criacao { get; set; } = DateTime.Now;
        public bool Lida { get; set; } = false;
        public string? Link { get; set; }
        public string? Tipo { get; set; } // success, warning, info, error
    }
}