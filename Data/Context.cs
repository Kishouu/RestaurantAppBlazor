using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;

namespace Restaurant.Data
{
  public class Context : DbContext
  {
    public Context(DbContextOptions<Context> options) :
      base(options)
    {
    }

    public DbSet<Dish> Dishes => Set<Dish>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Dish → Reviews (1:N)
      modelBuilder.Entity<Dish>()
        .HasMany(d => d.Reviews)
        .WithOne(r => r.Dish)
        .HasForeignKey(r => r.DishId);

      // User → Orders (1:N)
      modelBuilder.Entity<User>()
        .HasMany(u => u.Orders)
        .WithOne(o => o.User)
        .HasForeignKey(o => o.UserId);

      // User → Addresses (1:N)
      modelBuilder.Entity<User>()
        .HasMany(u => u.Addresses)
        .WithOne(a => a.User)
        .HasForeignKey(a => a.UserId);

      // Order → Items (1:N)
      modelBuilder.Entity<Order>()
        .HasMany(o => o.Items)
        .WithOne(i => i.Order)
        .HasForeignKey(i => i.OrderId);

      // Order → DeliveryAddress (0..1)
      modelBuilder.Entity<Order>()
        .HasOne(o => o.DeliveryAddress)
        .WithMany()
        .HasForeignKey(o => o.DeliveryAddressId);

      modelBuilder.Entity<Dish>().HasData(
        new Dish
        {
          DishId = 1, Name = "Margherita Pizza",
          Description = "Classic pizza with tomatoes, mozzarella, and basil.",
          Price = 8.99m, ImagePath = "images/pizza1.png"
        },
        new Dish
        {
          DishId = 2, Name = "Cheeseburger",
          Description =
            "Beef patty with cheese, lettuce, and tomato in a toasted bun.",
          Price = 7.49m, ImagePath = "images/burger1.png"
        },
        new Dish
        {
          DishId = 3, Name = "Caesar Salad",
          Description =
            "Romaine lettuce with Caesar dressing, croutons, and parmesan.",
          Price = 5.50m, ImagePath = "images/salad1.png"
        }
      );

      modelBuilder.Entity<User>().HasData(
        new User { UserId = 1, Name = "Alice" },
        new User { UserId = 2, Name = "Bob" }
      );

      modelBuilder.Entity<Address>().HasData(
        new Address
        {
          AddressId = 1, UserId = 1, Street = "123 Main St",
          City = "Springfield", PostalCode = "12345"
        },
        new Address
        {
          AddressId = 2, UserId = 2, Street = "456 Elm St", City = "Metropolis",
          PostalCode = "67890"
        }
      );
    }
  }

  public static class DbSeeder
  {
    public static void Seed(Context context)
    {
      if (!context.Dishes.Any())
      {
        context.Dishes.AddRange(
          new Dish
          {
            DishId = 1, Name = "Margherita Pizza",
            Description = "Classic pizza with tomatoes, mozzarella, and basil.",
            Price = 8.99m, ImagePath = "images/pizza1.jpg"
          },
          new Dish
          {
            DishId = 2, Name = "Cheeseburger",
            Description =
              "Beef patty with cheese, lettuce, and tomato in a toasted bun.",
            Price = 7.49m, ImagePath = "images/burger1.jpg"
          },
          new Dish
          {
            DishId = 3, Name = "Caesar Salad",
            Description =
              "Romaine lettuce with Caesar dressing, croutons, and parmesan.",
            Price = 5.50m, ImagePath = "images/salad1.jpg"
          }
        );
      }

      if (!context.Users.Any())
      {
        context.Users.AddRange(
          new User { UserId = 1, Name = "Alice" },
          new User { UserId = 2, Name = "Bob" }
        );
      }

      context.SaveChanges(); // ensure users/dishes are saved before inserting addresses/orders

      if (!context.Addresses.Any())
      {
        context.Addresses.AddRange(
          new Address
          {
            AddressId = 1, UserId = 1, Street = "123 Main St",
            City = "Springfield", PostalCode = "12345"
          },
          new Address
          {
            AddressId = 2, UserId = 2, Street = "456 Elm St",
            City = "Metropolis", PostalCode = "67890"
          }
        );
      }

      context.SaveChanges(); // ensure addresses exist before orders

      if (!context.Orders.Any())
      {
        context.Orders.Add(new Order
        {
          UserId = 1,
          OrderDate = DateTime.UtcNow.AddDays(-1),
          Status = OrderStatus.Delivered,
          IsDelivery = true,
          DeliveryAddressId = 1,
          DeliveryFee = 5.00m,
          TotalPrice = 13.99m // 8.99 + 5.00
        });

        context.SaveChanges();

        context.OrderItems.Add(new OrderItem
        {
          OrderItemId = 1,
          OrderId = 1,
          DishId = 1,
          Quantity = 1,
          UnitPrice = 8.99m,
          LineTotal = 8.99m
        });
      }

      if (!context.Reviews.Any())
      {
        context.Reviews.AddRange(
          new Review
          {
            ReviewId = 1,
            DishId = 1,
            UserId = 1,
            Rating = 5,
            Comment = "Delicious pizza!",
            ReviewDate = DateTime.UtcNow.AddDays(-1)
          },
          new Review
          {
            ReviewId = 2,
            DishId = 1,
            UserId = 2,
            Rating = 4,
            Comment = "Very good!",
            ReviewDate = DateTime.UtcNow.AddDays(-2)
          }
        );
      }

      context.SaveChanges();
    }
  }
}