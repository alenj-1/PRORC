using Microsoft.Extensions.DependencyInjection;
using PRORC.Domain.Interfaces.Integrations;
using PRORC.Domain.Interfaces.Logging;
using PRORC.Infrastructure.Integrations.Notifications;
using PRORC.Infrastructure.Integrations.Payments;
using PRORC.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRORC.Infrastructure.DependencyInjection
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<INotificationSender, EmailNotificationService>();
            services.AddScoped<IPaymentGateway, PaymentGatewayService>();
            services.AddScoped<IAuditLogger, AuditLogger>();

            return services;
        }
    }
}
