using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace POS.Orders.Data.Models
{
    public class OrderLineStatus
    {
        [Key]
        public byte Id { get; set; }

        public string? Status { get; set; }
    }
}
