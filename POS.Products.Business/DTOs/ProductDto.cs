using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using POS.CommonBase;

namespace POS.Products.Business.DTOs
{   
    public partial class ProductDto : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public ICollection<ProductSizeDto> Sizes { get; set; } = new List<ProductSizeDto>();
        //public ICollection<ProductCategoryDto> ProductCategories { get; set; } = new List<ProductCategoryDto>();

        public string? Description { get; set; }
        public string? OrderDescription { get; set; }
        public string? ReceiptDescription { get; set; }
        public string? LabelDescription { get; set; }
        public string? KitchenDescription { get; set; }
        public int DisplayOrder { get; set; }
        public short? IsTaxable { get; set; }
        public bool? IsPrepared { get; set; }
        public bool? IsPizza { get; set; }
        public bool? IsSpecialtyPizza { get; set; }
        public short StatusCode { get; set; }
        public bool ShouldPromptForSize { get; set; }
        public short ProductTypeCode { get; set; }
        public string? ProductImageUrl { get; set; }
        
    }

}
