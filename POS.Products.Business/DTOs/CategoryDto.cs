using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using POS.CommonBase;

namespace POS.Products.Business.DTOs
{   
    public partial class CategoryDto : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        // Relación muchos a muchos con Product
        public ICollection<ProductDto> Products { get; set; } = new List<ProductDto>();
    }

}
