using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Models;

namespace Restaurant.Services
{
    public class UserService
    {
        private readonly IDbContextFactory<Context> _factory;

        public UserService(IDbContextFactory<Context> factory)
        {
            _factory = factory;
        }

        public async Task<List<User>> GetAllAsync()
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.Include(u => u.Addresses).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            return await context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task AddOrUpdateAsync(User user)
        {
            using var context = _factory.CreateDbContext();
            if (user.UserId == 0)
            {
                context.Users.Add(user);
            }
            else
            {
                context.Users.Update(user);
            }
            await context.SaveChangesAsync(); 
        }

        public async Task DeleteAsync(int id)
        {
            using var context = _factory.CreateDbContext();
            var user = new User { UserId = id };
            context.Users.Attach(user);
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public async Task AddAddressAsync(int userId, Address address)
        {
            using var context = _factory.CreateDbContext();
            var user = await context.Users.Include(u => u.Addresses).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user != null)
            {
                address.UserId = userId;
                user.Addresses.Add(address);
                await context.SaveChangesAsync();
            }
        }
    }
}