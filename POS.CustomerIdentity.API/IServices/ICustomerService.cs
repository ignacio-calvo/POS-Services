using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.Models;

namespace POS.CustomerIdentity.API.IServices
{
    public interface ICustomerService
    {
        Task<CustomerResult> CreateCustomerAsync(CustomerDto customerDto, string token);
        Task<CustomerDto> GetCustomerByEmailAsync(string email, string token); 
    }
}
