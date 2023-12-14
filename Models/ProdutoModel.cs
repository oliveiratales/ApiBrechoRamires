using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiBrechoRamires.Models
{
    public class ProdutoModel
    {
        [JsonIgnore]
        [Key]
        public string? Codigo { get; set; }
        public required string Nome { get; set; }
        public required int Quantidade { get; set; }
        public string? Marca { get; set; }
        public required int Tipo { get; set; }
        public required string Categoria { get; set; }
        public required string Cor { get; set; }
        public required string Tamanho { get; set; }
        public decimal? PrecoPago { get; set; }
        public required decimal Preco { get; set; }
        public required int Origem { get; set; }
        public string? Dono { get; set; }

    }
}
