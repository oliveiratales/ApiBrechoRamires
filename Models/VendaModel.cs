using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ApiBrechoRamires.DTO;

namespace ApiBrechoRamires.Models
{
    public class VendaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public DateTime DataVenda { get; set; }
        public required decimal Valor { get; set; }
        public decimal? Desconto { get; set; }
        public required string FormaDePagamento { get; set; }
        public string? Cliente { get; set; }
        public required int Vendedor { get; set; }

        // POST e PUT (Somente código)
        [NotMapped]
        public List<string>? ListaProdutos { get; set; }

        // GET's (Todas informações)
        [JsonIgnore]
        public List<ProdutoModel>? ProdutosVendidos { get; set; }
    }
}