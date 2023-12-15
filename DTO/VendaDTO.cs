
using System.ComponentModel.DataAnnotations;

namespace ApiBrechoRamires.DTO
{
    public class VendaDTO
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DataVenda { get; set; }

        [Required]
        public decimal Valor { get; set; }

        public decimal? Desconto { get; set; }

        [Required]
        public required string FormaDePagamento { get; set; }

        public string? Cliente { get; set; }

        [Required]
        public int Vendedor { get; set; }

        public required List<ProdutoDTO> Produtos { get; set; }
    }
}