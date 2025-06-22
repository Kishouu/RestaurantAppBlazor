using System.ComponentModel.DataAnnotations;
namespace Restaurant.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        [Required]
        public string Street { get; set; } = null!;
        [Required]
        public string City { get; set; } = null!;
        [Required]
        public string PostalCode { get; set; } = null!;
    }
}