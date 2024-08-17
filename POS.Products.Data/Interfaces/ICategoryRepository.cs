
using POS.Products.Data.Interfaces;

namespace POS.Products.Data.Models
{
    public interface ICategoryRepository:IGenericRepository<Category>
    {
        Task<Category> GetByNameAsync(string name);
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<Category?>> GetAllCategoriesWithProductsAsync(bool tracked = true);
        Task<IEnumerable<Category?>> GetAllCategoriesWithProductsSizesAsync(bool tracked = true);
    }
}
