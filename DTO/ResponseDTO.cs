using System.ComponentModel.DataAnnotations;

namespace ApiBrechoRamires.DTO
{
    public class ResponseDTO
    {
        [Key]
        public required string Codigo { get; set; }

        public string? Mensagem {get; set;}
    }
}