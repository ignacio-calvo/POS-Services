using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.IServices;
using POS.CustomerRegistration.API.Models;
using System.Text.Json;

namespace POS.CustomerRegistration.API.Services
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
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");
                return new IdentityResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }

        public async Task DeleteIdentityAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/identity/{userId}");
            response.EnsureSuccessStatusCode();
        }
    }
}
