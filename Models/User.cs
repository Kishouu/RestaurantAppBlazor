using System.ComponentModel.DataAnnotations;
namespace Restaurant.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}