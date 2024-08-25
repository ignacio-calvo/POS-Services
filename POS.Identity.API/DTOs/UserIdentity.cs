namespace POS.Identity.API.DTOs
{
    public class UserIdentity
    {
        public string Email { get; set; }
        public string? Password { get; set; } // This can be null when the user logs in with SSO like Google
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
