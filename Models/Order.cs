using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Models
{
  public enum OrderStatus 
  { 
    Received, Confirmed, Preparing, InDelivery, Delivered, Canceled 
  }

  public class Order
  {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
    public int OrderId { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public bool IsDelivery { get; set; }
    public int? DeliveryAddressId { get; set; }
    public Address? DeliveryAddress { get; set; }

    [Range(0, 100)]
    public decimal DeliveryFee { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal TotalPrice { get; set; }
  }
}