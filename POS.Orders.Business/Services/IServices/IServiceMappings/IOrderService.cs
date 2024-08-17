using POS.Orders.Business.DTOs;
using POS.Orders.Data.Models;


namespace POS.Orders.Business.Services.IServices.IServiceMappings
{

    public interface IOrderService : IGenericServiceAsync<Order, OrderDto>
    {
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllByCustomerAsync(int customerId);
    }
}
