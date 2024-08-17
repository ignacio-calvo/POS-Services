using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS.Orders.Data.Models
{
    public class OrderType

    {
        [Key]
        public byte Id { get; set; }

        public string? Type { get; set; }
    }
}
