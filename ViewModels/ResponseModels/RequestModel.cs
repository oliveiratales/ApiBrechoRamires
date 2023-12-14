using System.ComponentModel.DataAnnotations;

namespace ApiBrechoRamires.ViewModels.ResponseModels
{
   public class RequestModel<T>
    {
        public T? Details { get; set; }

        [Required]   
        public short Status { get; set; }

        public string? Error { get; set; }

        public DateTimeOffset TimestampUtc { get; set; } = DateTimeOffset.Now;
    }
}