using POS.CommonBase;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace POS.Orders.Business.DTOs
{
    public class OrderLineDto : EntityBase
    {
        public int OrderId { get; set; } 
        public OrderDto Order { get; set; }

        [Key]
        public int Id { get; set; }
        public int Sequence { get; set; } //used to track sequential modifications to an order line and keep history of changes
        public bool Deleted { get; set; }
        public OrderLineStatusDto? Status { get; set; }
        public int Quantity { get; set; }
        public int? ProductId { get; set; }
        public int? ProductSizeId { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public bool BeingModified { get; set; }
        public string? Comments { get; set; }

    }


}
