using Microsoft.EntityFrameworkCore;
using Restaurant.Data;
using Restaurant.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Configure DbContext factory for PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<Context>(options =>
  options.UseNpgsql(connectionString));

// Register application services
builder.Services.AddScoped<DishService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Initialize and seed database
using (var scope = app.Services.CreateScope())
{
  var ctxFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<Context>>();
  using var context = ctxFactory.CreateDbContext();
  context.Database.EnsureCreated();
  DbSeeder.Seed(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();