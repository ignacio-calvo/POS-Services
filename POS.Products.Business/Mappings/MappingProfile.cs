using AutoMapper;
using POS.Products.Business.DTOs;
using POS.Products.Data.Models;

namespace POS.Products.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            //CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Product)));
            CreateMap<ProductSize, ProductSizeDto>().ReverseMap();
            //CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            
        }
    }
}
