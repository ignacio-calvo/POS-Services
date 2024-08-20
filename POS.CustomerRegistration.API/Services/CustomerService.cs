using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.IServices;
using POS.CustomerRegistration.API.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace POS.CustomerRegistration.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _httpClient;

        public CustomerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CustomerResult> CreateCustomerAsync(CustomerDto customerDto, string token) // Add token parameter
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(customerDto), Encoding.UTF8, "application/json");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsync("api/customers", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var customerResult = JsonSerializer.Deserialize<CustomerResult>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    customerResult.IsSuccess = response.IsSuccessStatusCode;
                    return customerResult;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                return new CustomerResult { IsSuccess = false, ErrorMessage = errorMessage };
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Exception: {ex.Message}");
                return new CustomerResult { IsSuccess = false, ErrorMessage = ex.Message };
            }
        }
    }
}
