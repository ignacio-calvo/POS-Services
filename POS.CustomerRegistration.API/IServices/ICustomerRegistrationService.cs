using Microsoft.AspNetCore.Mvc;
using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.Models;

namespace POS.CustomerRegistration.API.IServices
{
    public interface ICustomerRegistrationService
    {
        Task RegisterCustomerAsync(CustomerUserDto customerUserDto);
        Task<LoginResult> GoogleAuthAsync(GoogleLoginModel model);
    }
}
