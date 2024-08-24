using POS.Customers.Data.Models;

namespace POS.Customers.Data.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> Add(Customer customer);
        Task<Customer> GetById(int id);
        Task<Customer> GetByName(string name);
        Task<bool> Exists(int id);
        Task<IEnumerable<Customer>> GetAll();
        Task<Customer> Update(Customer customer);
        Task DeleteById(int id);
        Task SaveChangesAsync();
        Task<Customer> GetByEmail(string email);

    }
}
