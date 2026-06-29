// Models/Oferta.cs
namespace ToniEmprega.Models
{
    public class Oferta
    {
        public int Id { get; set; }

        public int Id_Empresa { get; set; }

        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        public Empresa Empresa { get; set; } = null!;

        public int? Id_Tipo_Oferta { get; set; }
        public TipoOferta? TipoOferta { get; set; }

        public int? Id_Estado_Oferta { get; set; }
        public EstadoOferta? EstadoOferta { get; set; }

        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;
        public string Localizacao { get; set; } = string.Empty;

        // 🗺️ NOVO: Coordenadas GPS para o mapa
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        public DateTime Data_Publicacao { get; set; } = DateTime.Now;
        public DateTime? Data_Expiracao { get; set; }

        // Navegação
        public ICollection<Candidatura> Candidaturas { get; set; } = new List<Candidatura>();
    

            // 🗺️ NOVO: Helper para resumir a morada (País, Distrito, Concelho, Rua)
        public string LocalizacaoResumida
        {
            get
            {
                if (string.IsNullOrEmpty(Localizacao)) return "Localização não definida";

                // Separar por vírgulas
                var partes = Localizacao.Split(',')
                    .Select(p => p.Trim())
                    .Where(p => !string.IsNullOrEmpty(p))
                    .ToList();

                if (partes.Count == 0) return Localizacao;

                // Pegar as partes mais importantes (geralmente as primeiras e últimas)
                // Formato típico: Rua, Número, Código Postal Cidade, Concelho, Distrito, País

                var resultado = new List<string>();

                // Rua (primeira parte)
                if (partes.Count >= 1)
                    resultado.Add(partes[0]);

                // Concelho/Cidade (procurar na parte do meio)
                for (int i = 1; i < partes.Count - 1; i++)
                {
                    var parte = partes[i];
                    // Ignorar números e códigos postais
                    if (!parte.All(char.IsDigit) && !parte.Contains('-') && parte.Length > 2)
                    {
                        if (!resultado.Contains(parte))
                            resultado.Add(parte);
                        break; // Só pegar o primeiro concelho/cidade
                    }
                }

                // Distrito (penúltima parte, se não for Portugal)
                if (partes.Count >= 2)
                {
                    var possivelDistrito = partes[partes.Count - 2];
                    if (!possivelDistrito.Contains("Portugal") && !resultado.Contains(possivelDistrito))
                        resultado.Add(possivelDistrito);
                }

                // País (última parte)
                var pais = partes.Last();
                if (!resultado.Contains(pais))
                    resultado.Add(pais);

                // Se ficou muito curto, mostrar as 3 primeiras partes
                if (resultado.Count < 2 && partes.Count >= 3)
                {
                    return string.Join(", ", partes.Take(3));
                }

                return string.Join(", ", resultado);
            }
        }
    }
}