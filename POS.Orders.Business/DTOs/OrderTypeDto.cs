using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS.Orders.Business.DTOs
{
    public class OrderTypeDto

    {
        [Key]
        public byte Id { get; set; }

        public string? Type { get; set; }
    }
}
