using Duende.IdentityServer.EntityFramework.Options;
using Duende.IdentityServer.Models;
using Infrastructure.Models;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Data;
public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
{
    private DbContextOptions<ApplicationDbContext> options;

    public DbSet<Message> Messages => Set<Message>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Latest> Latests => Set<Latest>();
    
    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    
}
