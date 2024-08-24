using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.IServices;
using POS.CustomerIdentity.API.Models;
using System.Text;
using System.Text.Json;

namespace POS.CustomerIdentity.API.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IdentityResult> RegisterIdentityAsync(RegisterModel registerModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/identity/register", registerModel);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<IdentityResult>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return new IdentityResult { IsSuccess = true, UserId = result.UserId };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new IdentityResult { IsSuccess = false, ErrorMessage = errorMessage };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new IdentityResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task DeleteIdentityAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/identity/{userId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<LoginResult> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(loginModel), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/identity/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LoginResult>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    loginResult.IsSuccess = response.IsSuccessStatusCode;
                    return loginResult;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return new LoginResult { IsSuccess = false, ErrorMessage = errorMessage };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new LoginResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<bool> IdentityExistsAsync(string email)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/identity/user-exists/{email}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<bool>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return false;
            }
        }

        public async Task<LoginResult> GoogleLoginAsync(GoogleLoginModel googleLoginModel)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(googleLoginModel), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/identity/google-login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<LoginResult>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    loginResult.IsSuccess = response.IsSuccessStatusCode;
                    return loginResult;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return new LoginResult { IsSuccess = false, ErrorMessage = errorMessage };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new LoginResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
    }
}
