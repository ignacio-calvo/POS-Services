using AutoMapper;
using POS.CustomerRegistration.API.DTOs;

namespace POS.CustomerRegistration.API.Mappings
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
