using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.Models;

namespace POS.CustomerIdentity.API.IServices
{
    public interface IGoogleService
    {
        Task<GoogleUserInfo> GetUserInfoAsync(string token);
    }
}
