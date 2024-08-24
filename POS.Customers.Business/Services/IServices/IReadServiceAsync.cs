namespace POS.Customers.Business.Services.IServices
{
    public interface IReadServiceAsync<TEntity, TDto>
            where TEntity : class
            where TDto : class
    {
        Task<IEnumerable<TDto>> GetAllAsync();
        Task<TDto> GetByIdAsync(int id);
        Task<bool> Exists(int id);
        Task<TDto> GetByEmailAsync(string email); // Add this line
    }
}
