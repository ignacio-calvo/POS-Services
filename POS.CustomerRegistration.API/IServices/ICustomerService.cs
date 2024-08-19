using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.Models;

namespace POS.CustomerRegistration.API.IServices
{
    public interface ICustomerService
    {
        Task<CustomerResult> CreateCustomerAsync(CustomerDto customerDto);

    }
}
