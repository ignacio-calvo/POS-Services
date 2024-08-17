using POS.CommonBase;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace POS.Products.Data.Models
{
    public class ProductSize : EntityBase
    {
        public int ProductId { get; set; } 
        public Product Product { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public short DisplayOrder { get; set; }
        public short StatusCode { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public bool DefaultSize { get; set; }
        public bool PriceByWeight { get; set; }
        public float TareWeight { get; set; }      

    }


}
