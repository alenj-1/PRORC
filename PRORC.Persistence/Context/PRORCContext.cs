using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRORC.Domain.Entities.Menus;
using PRORC.Domain.Entities.Notifications;
using PRORC.Domain.Entities.Orders;
using PRORC.Domain.Entities.Payments;
using PRORC.Domain.Entities.Reservations;
using PRORC.Domain.Entities.Restaurants;
using PRORC.Domain.Entities.Reviews;
using PRORC.Domain.Entities.Users;

namespace PRORC.Persistence.Context
{
    public class PRORCContext : DbContext
    {
        public PRORCContext(DbContextOptions<PRORCContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantAvailability> RestaurantAvailabilities { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
