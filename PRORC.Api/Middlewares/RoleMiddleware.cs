using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PRORC.Api.Middlewares
{
    public class RoleMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            var allowsAnonymous = endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null;
            if (allowsAnonymous)
            {
                await _next(context);
                return;
            }

            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value
                           ?? context.User.FindFirst("role")?.Value;

                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                             ?? context.User.FindFirst("sub")?.Value;

                if (!string.IsNullOrWhiteSpace(role))
                {
                    context.Items["CurrentUserRole"] = role;
                }

                if (!string.IsNullOrWhiteSpace(userId))
                {
                    context.Items["CurrentUserId"] = userId;
                }
            }

            await _next(context);
        }
    }
}