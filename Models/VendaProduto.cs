using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiBrechoRamires.Models
{
    public class VendaProduto
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Venda")]
        public int VendaId { get; set; }

        public VendaModel? Venda { get; set; }

        [ForeignKey("Produto")]
        public string? ProdutoCodigo { get; set; }

        public ProdutoModel? Produto { get; set; }

        public int Quantidade { get; set; }
    }
}
