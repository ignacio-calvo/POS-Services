using POS.Products.Business.DTOs;
using POS.Products.Data.Models;


namespace POS.Products.Business.Services.IServices.IServiceMappings
{

    public interface IProductService : IGenericServiceAsync<Product, ProductDto>
    {
        Task<bool> ExistsAsync(int id);
    }
}
