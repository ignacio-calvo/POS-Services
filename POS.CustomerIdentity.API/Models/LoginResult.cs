using POS.CustomerIdentity.API.DTOs;

namespace POS.CustomerIdentity.API.Models
{
    public class LoginResult
    {
        public bool IsSuccess { get; set; }
        public string Token { get; set; }
        public string ErrorMessage { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
