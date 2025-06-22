using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class OrderService
    {
        private readonly IDbContextFactory<Context> _factory;
        private const decimal DeliveryFeeFlat = 5.00m;

        public OrderService(IDbContextFactory<Context> factory)
        {
            _factory = factory;
        }

        public async Task<List<Order>> GetAllAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.Items).ThenInclude(i => i.Dish)
                .Include(o => o.DeliveryAddress)
                .ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.Items).ThenInclude(i => i.Dish)
                .Include(o => o.DeliveryAddress)
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task CreateAsync(Order order, List<OrderItem> items)
        {
            using var context = _factory.CreateDbContext();
            order.OrderDate = DateTime.UtcNow;
            order.Status = OrderStatus.Received;
            order.DeliveryFee = order.IsDelivery ? DeliveryFeeFlat : 0;

            foreach (var item in items)
            {
                item.Dish = null;
                item.LineTotal = item.UnitPrice * item.Quantity; 
            }

            order.Items = items;
            order.TotalPrice = items.Sum(i => i.LineTotal) + order.DeliveryFee;

            context.Orders.Add(order);
            await context.SaveChangesAsync();
        }
        public async Task UpdateStatusAsync(int orderId, OrderStatus newStatus)
        {
            using var context = _factory.CreateDbContext();
            var order = await context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int orderId)
        {
            using var context = _factory.CreateDbContext();
            var order = new Order { OrderId = orderId };
            context.Orders.Attach(order);
            context.Orders.Remove(order);
            await context.SaveChangesAsync();
        }
    }
}