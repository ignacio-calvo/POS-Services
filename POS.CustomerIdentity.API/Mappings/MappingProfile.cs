using AutoMapper;
using POS.CustomerIdentity.API.DTOs;

namespace POS.CustomerIdentity.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CustomerUserDto, CustomerDto>().ReverseMap();
            CreateMap<CustomerUserDto, RegisterModel>().ReverseMap();
        }
    }
}
