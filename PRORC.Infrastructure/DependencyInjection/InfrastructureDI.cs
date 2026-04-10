using Microsoft.Extensions.DependencyInjection;
using PRORC.Domain.Interfaces.Integrations;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Infrastructure.Integrations.Notifications;
using PRORC.Infrastructure.Integrations.Payments;
using PRORC.Infrastructure.Logging;
using Microsoft.Extensions.Configuration;

namespace PRORC.Infrastructure.DependencyInjection
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<INotificationSender, EmailNotificationService>();
            services.AddTransient<IPaymentGateway, PaymentGatewayService>();
            services.AddTransient<IAuditLogger, AuditLogger>();

            return services;
        }
    }
}