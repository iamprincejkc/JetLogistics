using JetLogistics.Identity.API.Models;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JetLogistics.Identity.API.DataAccess;
using JetLogistics.Identity.API.Services;

namespace JetLogistics.Identity.API.Services
{
    public static class OpenIddictSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();
            var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

            await context.Database.EnsureCreatedAsync();

            // Seed default user
            if (!await context.Users.AnyAsync(u => u.Username == "admin"))
            {
                var user = new ApplicationUser
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    FullName = "Administrator",
                    PasswordHash = PasswordHasherService.HashPassword("password123")
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            // Seed clients
            await CreateClientAsync(appManager, "clientid", "clientsecret", new[] { "offline_access", "consignee_api" }, passwordGrant: true);
            await CreateClientAsync(appManager, "clientid2", "clientsecret2", new[] { "offline_access","booking_api" }, passwordGrant: true);
            await CreateClientAsync(appManager, "clientid0", "clientsecret0", new[] { "offline_access", "consignee_api", "booking_api" }, passwordGrant: true);

            // Seed scopes
            await CreateScopeAsync(scopeManager, "consignee_api", "Access to Consignee", new[] { "consignee_audience", "gateway" ,"booking_audience" });
            await CreateScopeAsync(scopeManager, "booking_api", "Access to Consignee", new[] { "consignee_audience", "gateway", "booking_audience" });
        }

        private static async Task CreateClientAsync(IOpenIddictApplicationManager manager, string clientId, string secret, string[] scopes, bool passwordGrant)
        {
            if (await manager.FindByClientIdAsync(clientId) is not null)
                return;

            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = clientId,
                ClientSecret = secret,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token
                }
            };

            if (passwordGrant)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.Password);
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);

            }

            foreach (var scope in scopes)
            {
                descriptor.Permissions.Add(OpenIddictConstants.Permissions.Prefixes.Scope + scope);
            }

            await manager.CreateAsync(descriptor);
        }

        private static async Task CreateScopeAsync(IOpenIddictScopeManager manager, string name, string displayName, IEnumerable<string> resources)
        {
            if (await manager.FindByNameAsync(name) is not null)
                return;

            var descriptor = new OpenIddictScopeDescriptor
            {
                Name = name,
                DisplayName = displayName
            };

            foreach (var resource in resources)
            {
                descriptor.Resources.Add(resource);
            }

            await manager.CreateAsync(descriptor);
        }
    }
}
