using POS.CustomerIdentity.API.DTOs;

namespace POS.CustomerIdentity.API.Models
{
    public class CustomerResult
    {
        public bool IsSuccess { get; set; }
        public CustomerDto Customer { get; set; }
        public string ErrorMessage { get; set; }
    }
}
