using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using POS.CommonBase;
using Microsoft.EntityFrameworkCore;

namespace POS.Stores.Business.DTOs
{
    public class StoreDto : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public required string Name { get; set; }
        public bool IsDefault { get; set; } //used to mark the default store for a tenant with multiple stores. Should be just one default store per tenant used to display menu when no store was selected by user
        public string? PhoneNumber { get; set; }
        public string? AddressLine1 { get; set; }
        public string? StreetNumber { get; set; }
        public string? AddressLine2 { get; set; }
        public string? State{ get; set; }
        public string? City { get; set; }
        public string? PostalCode { get; set; }
        public string? Email { get; set; }         

    }
}
