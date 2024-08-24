using Microsoft.AspNetCore.Mvc;
using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.Models;

namespace POS.CustomerIdentity.API.IServices
{
    public interface ICustomerIdentityService
    {
        Task RegisterCustomerAsync(CustomerUserDto customerUserDto);
        Task<LoginResult> GoogleAuthAsync(GoogleLoginModel model);
    }
}
