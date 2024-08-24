using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using POS.CommonBase;

namespace POS.CustomerIdentity.API.DTOs
{
    public class CustomerDto:EntityBase
    {
        [Key]
        public int Id { get; set; }

        public required string Email { get; set; }
        public required string LastName { get; set; }
        public required string FirstName { get; set; } 
        public string PhoneNumber { get; set; }
        public string? PhoneExtension { get; set; }
        public string? CompanyName { get; set; }
        public string? AddressLine1 { get; set; }
        public string? StreetNumber { get; set; }
        public string? AddressLine2 { get; set; }
        public string? State{ get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }

    }
}
