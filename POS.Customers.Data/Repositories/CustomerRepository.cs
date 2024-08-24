using POS.Customers.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using POS.Customers.Data.Models;

namespace POS.Customers.Data.Repositories
{
    // Generic repository handling typical CRUD methods.
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDbContext _context;

        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Add(Customer customer)
        {
            var result = await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Customer> GetById(int id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task DeleteById(int id)
        {
            var result = await _context.Customers
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Customers.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Customer> Update(Customer customer)
        {
            if (customer != null)
            {
                _context.Customers.Update(customer);
                await _context.SaveChangesAsync();
                return customer;
            }
            return null;
        }

        public async Task<Customer> GetByName(string name)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(e => e.FirstName == name);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Customers
                .AnyAsync(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> GetByEmail(string email)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(e => e.Email == email);
        }
    }
}
