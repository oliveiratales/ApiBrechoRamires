using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiBrechoRamires.DTO
{
    public class LoginDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Nome { get; set; }
        [Required]
        public required string Senha { get; set; }
    }
}