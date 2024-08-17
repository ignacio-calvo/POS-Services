using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using POS.CommonBase;
using Microsoft.EntityFrameworkCore;

namespace POS.Orders.Data.Models
{   
    public partial class Order : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
                
        public ICollection<OrderLine> orderLines { get; set; } = new List<OrderLine>();
        public int? CustomerId { get; set; }
        public DateTime Date { get; set; }
        public bool BeingModified { get; set; }
        public bool DelayedOrder { get; set; } //mark down if order is ASAP or needs to be delayed
        public DateTime ExpectedDate { get; set; } //datetime in which order is expected to be delivered to customer
        public OrderStatus? Status { get; set; }
        public OrderType? OrderType { get; set; }
        public string? Comments { get; set; }
        [Precision(8, 2)]
        public decimal DeliveryFee { get; set; }
        [Precision(18, 2)]
        public decimal SubTotal { get; set; }
        [Precision(18, 2)]
        public decimal FinalTotal { get; set; }
        public DateTime RoutedOutDate { get; set; } //datetime in which order went out the store to customer's destination
        public DateTime DeliveredDate { get; set; } //datetime in which order was effectively delivered to customer
        public DateTime KitchenDisplayDate { get; set; } //datetime in which order needs to start being displayed in kitchen for cooking it


    }

}
