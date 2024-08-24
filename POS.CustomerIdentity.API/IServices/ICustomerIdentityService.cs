using Microsoft.AspNetCore.Mvc;
using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.Models;

namespace POS.CustomerIdentity.API.IServices
{
    public interface ICustomerIdentityService
    {
        Task<LoginResult> RegisterCustomerAsync(CustomerUserDto userRegistrationDto);
        Task<LoginResult> GoogleAuthAsync(GoogleLoginModel model);
        Task<LoginResult> LoginAsync(LoginModel model);
    }
}
