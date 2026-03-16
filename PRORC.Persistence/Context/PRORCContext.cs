using Microsoft.EntityFrameworkCore;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Entities.Reviews;
using PRORC.Domain.Entities.Users;

namespace PRORC.Persistence.Context;

public class PRORCContext : DbContext
{
    public PRORCContext(DbContextOptions<PRORCContext> options) : base(options) { }

    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<RestaurantAvailability> RestaurantAvailabilities => Set<RestaurantAvailability>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PRORCContext).Assembly);
        }
    }
