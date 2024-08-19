using POS.CustomerRegistration.API.DTOs;

namespace POS.CustomerRegistration.API.Models
{
    public class CustomerResult
    {
        public bool IsSuccess { get; set; }
        public CustomerDto Customer { get; set; }
        public string ErrorMessage { get; set; }
    }
}
