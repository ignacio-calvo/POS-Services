﻿using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.Models;

namespace POS.CustomerRegistration.API.IServices
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
