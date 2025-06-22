using System.ComponentModel.DataAnnotations;
namespace Restaurant.Models
{
    public class Dish
    {
        public int DishId { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0.01, 1000)]
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}