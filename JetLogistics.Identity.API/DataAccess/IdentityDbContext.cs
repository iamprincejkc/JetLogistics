using JetLogistics.Identity.API.Models;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;

namespace JetLogistics.Identity.API.DataAccess
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        // OpenIddict tables
        public DbSet<OpenIddictEntityFrameworkCoreApplication> Applications => Set<OpenIddictEntityFrameworkCoreApplication>();
        public DbSet<OpenIddictEntityFrameworkCoreAuthorization> Authorizations => Set<OpenIddictEntityFrameworkCoreAuthorization>();
        public DbSet<OpenIddictEntityFrameworkCoreScope> Scopes => Set<OpenIddictEntityFrameworkCoreScope>();
        public DbSet<OpenIddictEntityFrameworkCoreToken> Tokens => Set<OpenIddictEntityFrameworkCoreToken>();

        //account
        public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    }
}
