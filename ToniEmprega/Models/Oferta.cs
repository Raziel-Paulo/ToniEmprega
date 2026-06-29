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


        // 🗺️ NOVO: Helper para resumir a morada (País, Distrito, Concelho)
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

                // Formato típico: Rua/Número, Código Postal Cidade, Concelho, Distrito, País
                // Queremos: País, Distrito, Concelho (ordem invertida, das partes do fim)

                var resultado = new List<string>();

                // Pegar as últimas 3 partes úteis (de trás para frente)
                var partesRelevantes = new List<string>();

                foreach (var parte in partes)
                {
                    // Ignorar números de porta, códigos postais, etc.
                    var parteLimpa = parte.Trim();

                    // Se for só números ou código postal (ex: "7240-229", "100"), ignorar
                    if (parteLimpa.All(char.IsDigit)) continue;
                    if (parteLimpa.Contains('-') && parteLimpa.Length <= 10 && parteLimpa.Any(char.IsDigit)) continue;

                    // Adicionar se não for duplicado
                    if (!partesRelevantes.Contains(parteLimpa, StringComparer.OrdinalIgnoreCase))
                        partesRelevantes.Add(parteLimpa);
                }

                // Pegar as últimas 3 partes (País, Distrito, Concelho/Cidade)
                // Ordem no array: [Rua, Cidade, Concelho, Distrito, País]
                // Queremos: País, Distrito, Concelho

                if (partesRelevantes.Count >= 1)
                    resultado.Add(partesRelevantes.Last()); // País

                if (partesRelevantes.Count >= 2)
                    resultado.Add(partesRelevantes[partesRelevantes.Count - 2]); // Distrito

                if (partesRelevantes.Count >= 3)
                {
                    // Concelho/Cidade — pegar a parte antes do distrito
                    var concelho = partesRelevantes[partesRelevantes.Count - 3];
                    // Se for igual ao distrito, tentar a anterior
                    if (concelho.Equals(resultado[1], StringComparison.OrdinalIgnoreCase) && partesRelevantes.Count >= 4)
                        concelho = partesRelevantes[partesRelevantes.Count - 4];
                    resultado.Add(concelho);
                }

                // Se não conseguimos extrair nada útil, mostrar as últimas 2 partes
                if (resultado.Count == 0 && partes.Count >= 2)
                {
                    return string.Join(", ", partes.TakeLast(2));
                }

                // Resultado: País, Distrito, Concelho
                return string.Join(", ", resultado);
            }
        }
    }
}