using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [Required]
        public decimal Valor { get; set; }

        public decimal? Desconto { get; set; }

        [Required]
        public required string FormaDePagamento { get; set; }

        public string? Cliente { get; set; }

        [Required]
        public int Vendedor { get; set; }

        // POST e PUT (Somente código)
        [NotMapped]
        public List<ProdutoQuantidadeDTO>? ListaProdutos { get; set; }

        // GET's (Todas informações)
        [JsonIgnore]
        public List<ProdutoModel>? ProdutosVendidos { get; set; }

        // Tabela de junção
        [JsonIgnore]
        public List<VendaProduto>? VendaProdutos { get; set; } = new List<VendaProduto>();
    }

    public class ProdutoQuantidadeDTO
    {
        public required string Codigo { get; set; }
        public int Quantidade { get; set; }
    }
}
