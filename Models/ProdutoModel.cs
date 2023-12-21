using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiBrechoRamires.Models
{
    public class ProdutoModel
    {
        [JsonIgnore]
        [Key]
        public string? Codigo { get; set; }

        [Required]
        public required string Nome { get; set; }

        [Required]
        public int Quantidade { get; set; }

        public string? Marca { get; set; }

        [Required]
        public int Tipo { get; set; }

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

        public string? Dono { get; set; }

        [JsonIgnore]
        public List<VendaProduto>? VendasAssociadas { get; set; }

        // Tabela de Junção
        [JsonIgnore]
        public List<VendaProduto>? VendaProdutos { get; set; } = new List<VendaProduto>();
    }
}
