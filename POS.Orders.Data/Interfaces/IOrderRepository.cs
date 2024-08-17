using POS.Orders.Data.Interfaces;
using POS.Orders.Data.Models;

namespace POS.Orders.Data.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IEnumerable<Order>> GetAllByCustomerAsync(int customerId);
        Task<bool> ExistsAsync(int id);
    }
}
