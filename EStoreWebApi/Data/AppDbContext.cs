using EStoreWebApi.Features.Accounts.Entities;
using EStoreWebApi.Features.Cart.Entities;
using EStoreWebApi.Features.Catalogue.Entities;
using EStoreWebApi.Shared.Entities;
using EStoreWebApi.Shared.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EStoreWebApi.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartLineItem> CartLineItems => Set<CartLineItem>();
    // public DbSet<Country> Countries => Set<Country>();
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.StateChanged += UpdateTimestamps;
        ChangeTracker.Tracked += UpdateTimestamps;

        ChangeTracker.StateChanged += HandleAutoCreateAndDeleteCarts;
        ChangeTracker.Tracked += HandleAutoCreateAndDeleteCarts;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connection = "host=127.0.0.1;user=root;password=;database=dotnet_estore_dev";
        var serverVersion = new MySqlServerVersion("8.0.27");

        options
            .UseMySql(connection, serverVersion)
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
    }

    private void UpdateTimestamps(object? sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is TimestampedEntity entity)
        {
            entity.UpdatedAt = DateTime.Now;

            if (e.Entry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.Now;
            }
        }
    }
    
    private void HandleAutoCreateAndDeleteCarts(object? sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is User user)
        {
            if (e.Entry.State == EntityState.Added)
            {
                Carts.Add(new Cart()
                {
                    User = user,
                });
            }

            if (e.Entry.State == EntityState.Deleted)
            {
                var cart = Carts
                    .Include(c => c.CartLineItems)
                    .FirstOrDefault(c => c.UserId == user.Id);

                if (cart is not null)
                {
                    CartLineItems.RemoveRange(cart.CartLineItems);
                    Carts.Remove(cart);
                }
            }
        }
    }
}
