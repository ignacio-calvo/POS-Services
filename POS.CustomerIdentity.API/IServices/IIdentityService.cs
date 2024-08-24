using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.Models;

namespace POS.CustomerIdentity.API.IServices
{
    public interface IIdentityService
    {
        Task<IdentityResult> RegisterIdentityAsync(RegisterModel registerModel);
        Task DeleteIdentityAsync(string userId);
        Task<LoginResult> LoginAsync(LoginModel loginModel);
        Task<bool> IdentityExistsAsync(string email);
        Task<LoginResult> GoogleLoginAsync(GoogleLoginModel googleLoginModel);
    }
}
