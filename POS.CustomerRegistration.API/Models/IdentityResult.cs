namespace POS.CustomerRegistration.API.Models
{
    public class IdentityResult
    {
        public bool IsSuccess { get; set; }
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
    }
}
