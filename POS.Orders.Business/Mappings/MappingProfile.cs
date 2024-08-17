using AutoMapper;
using POS.Orders.Business.DTOs;
using POS.Orders.Data.Models;

namespace POS.Orders.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderLine, OrderLineDto>().ReverseMap();
            CreateMap<OrderLineStatus, OrderLineStatusDto>().ReverseMap();
            CreateMap<OrderStatus, OrderStatusDto>().ReverseMap();
            CreateMap<OrderType, OrderTypeDto>().ReverseMap();
        }
    }
}
