using POS.Stores.Data.Models;

namespace POS.Stores.Data.Interfaces
{
    public interface IStoreRepository
    {
        Task<Store> Add(Store Store);
        Task<Store> GetById(int id);
        Task<Store> GetByName(string name);
        Task<bool> Exists(int id);
        Task<IEnumerable<Store>> GetAll();
        Task<Store> Update(Store Store);
        Task DeleteById(int id);
        Task SaveChangesAsync();

    }
}
