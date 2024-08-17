using AutoMapper;
using POS.Orders.Business.CustomExceptions;
using POS.Orders.Business.DTOs;
using POS.Orders.Business.Services.IServices.IServiceMappings;
using POS.Orders.Data.Interfaces;
using POS.Orders.Data.Models;

namespace POS.Orders.Business.Services.ServiceMappings
{
    public class OrderService : GenericServiceAsync<Order, OrderDto>, IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public OrderService(IOrderRepository repository, IMapper mapper) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                var result = await _repository.ExistsAsync(id);

                if (!result)
                {
                    throw new EntityNotFoundException($"Entity with ID {id} not found.");
                }

                return result;
            }

            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving {typeof(OrderDto).Name} with Id: {id}";

                throw new EntityNotFoundException(message, ex);
            }
        }
        public async Task<IEnumerable<OrderDto>> GetAllByCustomerAsync(int customerId)
        {
            try
            {
                var result = await _repository.GetAllByCustomerAsync(customerId);

                if (result.Any())
                {
                    return _mapper.Map<IEnumerable<OrderDto>>(result);
                }
                else
                {
                    throw new EntityNotFoundException($"No {typeof(OrderDto).Name}s were found");
                }

            }
            catch (EntityNotFoundException ex)
            {
                var message = $"Error retrieving all {typeof(OrderDto).Name}s";

                throw new EntityNotFoundException(message, ex);
            }
        }
    }
} 
