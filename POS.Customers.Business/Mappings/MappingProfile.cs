using AutoMapper;
using POS.Customers.Business.DTOs;
using POS.Customers.Data.Models;

namespace POS.Customers.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
        }
    }
}
