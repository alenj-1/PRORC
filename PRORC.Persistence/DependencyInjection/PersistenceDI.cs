using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PRORC.Domain.Interfaces.Repositories;
using PRORC.Persistence.Context;
using PRORC.Persistence.Repositories.Menus;
using PRORC.Persistence.Repositories.Notifications;
using PRORC.Persistence.Repositories.Orders;
using PRORC.Persistence.Repositories.Payments;
using PRORC.Persistence.Repositories.Reservations;
using PRORC.Persistence.Repositories.Restaurants;
using PRORC.Persistence.Repositories.Reviews;
using PRORC.Persistence.Repositories.Users;

namespace PRORC.Persistence.DependencyInjection
{
    public static class PersistenceDI
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");
            }

            // Registro de DbContext con SQL Server
            services.AddDbContext<PRORCContext>(options =>
            {
                options.UseSqlServer(connectionString);
                options.EnableDetailedErrors();
            });

            // Registro de cada repositorio con su interfaz
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}