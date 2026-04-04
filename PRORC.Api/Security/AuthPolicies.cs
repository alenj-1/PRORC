using Microsoft.Extensions.DependencyInjection;

namespace PRORC.Api.Security
{
    public static class AuthPolicies
    {
        public const string CustomerOnly = "CustomerOnly";
        public const string RestaurantAdminOnly = "RestaurantAdminOnly";
        public const string SystemAdminOnly = "SystemAdminOnly";
        public const string RestaurantOrSystemAdmin = "RestaurantOrSystemAdmin";

        public static IServiceCollection AddAppAuthorization(this IServiceCollection services)
        {
            var builder = services.AddAuthorizationBuilder();
           
            builder.AddPolicy(CustomerOnly, policy =>
                policy.RequireRole("Customer"));

            builder.AddPolicy(RestaurantAdminOnly, policy =>
                policy.RequireRole("RestaurantAdmin"));

            builder.AddPolicy(SystemAdminOnly, policy =>
                policy.RequireRole("SystemAdmin"));

            builder.AddPolicy(RestaurantOrSystemAdmin, policy =>
                policy.RequireRole("RestaurantAdmin", "SystemAdmin"));

            return services;
        }
    }
}
