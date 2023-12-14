using System.ComponentModel.DataAnnotations;

namespace ApiBrechoRamires.DTO
{
    public class PostProdutoDTO
    {
        [Key]
        public required string Codigo { get; set; }

        public string? Mensagem {get; set;}
    }
}