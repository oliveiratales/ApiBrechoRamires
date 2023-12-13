using System.ComponentModel.DataAnnotations;

namespace ApiBrechoRamires.Models
{
    public class ProdutoModel
    {
        [Key]
        public required string Codigo { get; set; }

        public required string Nome { get; set; }
        public required int Quantidade { get; set; }
        public string? Marca { get; set; }
        public required string Categoria { get; set; }
        public required string Cor { get; set; }
        public required string Tamanho { get; set; }
        public decimal? PrecoPago { get; set; }
        public required decimal Preco { get; set; }
        public required int Origem { get; set; }

    }
}
