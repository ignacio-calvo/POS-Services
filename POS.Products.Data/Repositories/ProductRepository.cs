using Microsoft.EntityFrameworkCore;
using POS.Products.Data.Models;

namespace POS.Products.Data.Repositories
{
    //Generic repository handling typical CRUD methods.
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddAsync(Product Product)
        {
            var result = await _context.Products.AddAsync(Product);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Product?>> GetAllAsync(bool tracked = true)
        {
            return await _context.Products
                .Include(p => p.Sizes)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var result = await _context.Products
                .Include(p => p.Sizes)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Products.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Product> UpdateAsync(Product Product)
        {
            if (Product != null)
            {
                _context.Products.Update(Product);
                await _context.SaveChangesAsync();
                return Product;
            }
            return null;
        }
        public async Task<Product> GetByNameAsync(string name)
        {
            return await _context.Products
                .Include(p => p.Sizes)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(e => e.Name == name);
        }


        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Products.AnyAsync(e => e.Id== id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
