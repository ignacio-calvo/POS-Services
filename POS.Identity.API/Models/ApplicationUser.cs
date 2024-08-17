using Microsoft.AspNetCore.Identity;

namespace POS.Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional properties if needed
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
