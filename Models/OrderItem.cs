using System.ComponentModel.DataAnnotations;
namespace Restaurant.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        public int DishId { get; set; }
        public Dish? Dish { get; set; }
        [Range(1, 100)]
        public int Quantity { get; set; }
        [Range(0.01,1000)]
        public decimal UnitPrice { get; set; }
        public decimal LineTotal { get; set; }
    }
}