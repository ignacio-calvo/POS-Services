using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.Models;

namespace POS.CustomerRegistration.API.IServices
{
    public interface IGoogleService
    {
        Task<GoogleUserInfo> GetUserInfoAsync(string token);
    }
}
