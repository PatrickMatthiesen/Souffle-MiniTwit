using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MiniTwit.Infrastructure.Models;

namespace MiniTwit.Infrastructure.DbContext;
public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser> {
    private DbContextOptions<ApplicationDbContext> options;

    public DbSet<Message> Messages => Set<Message>();
    public DbSet<ApplicationUser> Users => Set<ApplicationUser>();
    public DbSet<Latest> Latests => Set<Latest>();

    public ApplicationDbContext(
        DbContextOptions options,
        IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions) {
    }
}
