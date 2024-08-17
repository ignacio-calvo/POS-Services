
using POS.Products.Data.Interfaces;

namespace POS.Products.Data.Models
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        Task<Product> GetByNameAsync(string name);
        Task<bool> ExistsAsync(int id);
    }
}
