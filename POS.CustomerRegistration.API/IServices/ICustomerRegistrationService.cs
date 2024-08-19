using POS.CustomerRegistration.API.DTOs;

namespace POS.CustomerRegistration.API.IServices
{
    public interface ICustomerRegistrationService
    {
        Task RegisterCustomerAsync(CustomerUserDto customerUserDto);
    }
}
