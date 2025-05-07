using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace JetLogistics.Gateway.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseScopeClaimSplitter(this IApplicationBuilder app)
        {
            return app.Use(async (context, next) =>
            {
                var scopeClaim = context.User?.FindFirst("scope");

                if (scopeClaim != null && scopeClaim.Value.Contains(" "))
                {
                    var identity = (ClaimsIdentity)context.User.Identity;
                    identity.RemoveClaim(scopeClaim);

                    foreach (var scope in scopeClaim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                    {
                        identity.AddClaim(new Claim("scope", scope));
                    }
                }

                await next();
            });
        }
    }
}
