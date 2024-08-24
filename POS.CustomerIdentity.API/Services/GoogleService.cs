using POS.CustomerIdentity.API.DTOs;
using POS.CustomerIdentity.API.IServices;
using POS.CustomerIdentity.API.Models;
using System.Text;
using System.Text.Json;

namespace POS.CustomerIdentity.API.Services
{
    public class GoogleService : IGoogleService
    {
        public GoogleService()
        {
            
        }

        public async Task<GoogleUserInfo> GetUserInfoAsync(string token)
        {
            try
            {
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"https://www.googleapis.com/oauth2/v1/userinfo?access_token={token}");
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var googleUserInfo = JsonSerializer.Deserialize<GoogleUserInfo>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    return googleUserInfo;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorMessage}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }

    }
}
