using AutoMapper;
using POS.Stores.Business.DTOs;
using POS.Stores.Data.Models;

namespace POS.Stores.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Store, StoreDto>().ReverseMap();
        }
    }
}
