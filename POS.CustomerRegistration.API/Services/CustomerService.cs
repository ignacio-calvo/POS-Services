using POS.CustomerRegistration.API.DTOs;
using POS.CustomerRegistration.API.IServices;
using POS.CustomerRegistration.API.Models;
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

        public async Task<CustomerResult> CreateCustomerAsync(CustomerDto customerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/customers", customerDto);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var createdCustomer = JsonSerializer.Deserialize<CustomerDto>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return new CustomerResult { IsSuccess = true, Customer = createdCustomer };
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return new CustomerResult { IsSuccess = false, ErrorMessage = errorMessage };
            }
        }
    }
}
