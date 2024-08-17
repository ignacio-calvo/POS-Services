using Microsoft.EntityFrameworkCore;
using POS.Orders.Data.Interfaces;
using POS.Orders.Data.Models;

namespace POS.Orders.Data.Repositories
{
    //Generic repository handling typical CRUD methods.
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        
        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order order)
        {
            var result = await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Order?>> GetAllAsync(bool tracked = true)
        {
            return await _context.Orders
                .Include(p => p.orderLines)
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var result = await _context.Orders
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Orders.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Order> UpdateAsync(Order order)
        {
            if (order != null)
            {
                _context.Orders.Update(order);
                await _context.SaveChangesAsync();
                return order;
            }
            return null;
        }
        public async Task<IEnumerable<Order>> GetAllByCustomerAsync(int customerId)
        {
            return await _context.Orders
                .Include(p => p.orderLines)
                .Where(e => e.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Orders.AnyAsync(e => e.Id== id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
