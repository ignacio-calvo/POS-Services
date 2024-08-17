using Microsoft.EntityFrameworkCore;
using POS.Products.Data.Interfaces;
using POS.Products.Data.Models;

namespace POS.Products.Data.Repositories
{
    //Generic repository handling typical CRUD methods.
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductDbContext _context;
        
        public CategoryRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddAsync(Category category)
        {
            var result = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Category?>> GetAllAsync(bool tracked = true)
        {
            return await _context.Categories
                .ToListAsync();
        }

        public async Task<IEnumerable<Category?>> GetAllCategoriesWithProductsAsync(bool tracked = true)
        {
            return await _context.Categories
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category?>> GetAllCategoriesWithProductsSizesAsync(bool tracked = true)
        {
            return await _context.Categories
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .ThenInclude(pcs => pcs.Sizes)
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            var result = await _context.Categories
                .Include(p => p.ProductCategories)                
                .FirstOrDefaultAsync(e => e.Id == id);
            if (result != null)
            {
                _context.Categories.Remove(result);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Category> UpdateAsync(Category category)
        {
            if (category != null)
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return category;
            }
            return null;
        }
        public async Task<Category> GetByNameAsync(string name)
        {
            return await _context.Categories
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Product)
                .FirstOrDefaultAsync(e => e.Name == name);
        }


        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Categories.AnyAsync(e => e.Id== id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
