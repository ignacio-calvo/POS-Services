using POS.Stores.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using POS.Stores.Data.Models;

namespace POS.Stores.Data.Repositories
{
    // Generic repository handling typical CRUD methods.
    public class StoreRepository : IStoreRepository
    {
        private readonly StoreDbContext _context;

        public StoreRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<Store> Add(Store Store)
        {
            var result = await _context.Stores.AddAsync(Store);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Store> GetById(int id)
        {
            return await _context.Stores
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Store>> GetAll()
        {
            return await _context.Stores.ToListAsync();
        }

        public async Task DeleteById(int id)
        {
            var result = await _context.Stores
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Stores.Remove(result);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Store> Update(Store Store)
        {
            if (Store != null)
            {
                _context.Stores.Update(Store);
                await _context.SaveChangesAsync();
                return Store;
            }
            return null;
        }

        public async Task<Store> GetByName(string name)
        {
            return await _context.Stores
                .FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Stores
                .AnyAsync(e => e.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
