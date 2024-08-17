using POS.Products.Business.DTOs;
using POS.Products.Data.Models;


namespace POS.Products.Business.Services.IServices.IServiceMappings
{
    public interface ICategoryService : IGenericServiceAsync<Category, CategoryDto>
    {
        Task<bool> ExistsAsync(int id);
        Task<IEnumerable<CategoryDto>> GetAllWithProductsAsync();
        Task<IEnumerable<CategoryDto>> GetAllWithProductsSizesAsync();
    }
}
