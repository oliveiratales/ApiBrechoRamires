using System.ComponentModel.DataAnnotations;

namespace ApiBrechoRamires.DTO
{
    public class ProdutoDTO
    {
        [Key]
        public required string Codigo { get; set; }

        [Required]
        public required string Nome { get; set; }

        [Required]
        public required int Quantidade { get; set; }

        public string? Marca { get; set; }

        [Required]
        public required string Categoria { get; set; }

        [Required]
        public required string Cor { get; set; }

        [Required]
        public required string Tamanho { get; set; }

        public decimal? PrecoPago { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        public int Origem { get; set; }
    }
}
