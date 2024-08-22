using AutoMapper;
using POS.Products.Business.DTOs;
using POS.Products.Data.Models;

namespace POS.Products.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => (ProductType)src.ProductType))
                .ReverseMap()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => (int)src.ProductType));

            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.Product)));

            CreateMap<ProductSize, ProductSizeDto>().ReverseMap();
        }
    }
}
