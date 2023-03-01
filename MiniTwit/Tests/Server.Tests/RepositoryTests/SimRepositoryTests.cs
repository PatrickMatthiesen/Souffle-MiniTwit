using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;

namespace Server.Tests.RepositoryTests;
public sealed class SimRepositoryTests : IAsyncDisposable {

    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly SimRepository _repository;

    public SimRepositoryTests() {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlite(_connection);

        OperationalStoreOptions storeOptions = new OperationalStoreOptions { };
        IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

        _context = new ApplicationDbContext(builder.Options, operationalStoreOptions);
        _context.Database.EnsureCreated();

        _repository = new SimRepository(_context);
    }

    [Fact]
    public async Task GetAsync_Succes() {
        var expected = new LatestDTO { latest = 0 };
        var actual = await _repository.GetAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task RegisterUser_And_SendMessage_Then_CheckLatest() {
        var user = new SimUserDTO { userName = "Asger" };
        var expected = Response.NoContent;
        var actual = await _repository.RegisterUser(user);

        Assert.Equal(expected, actual);

        var message = new SimMessageDTO { content = "Hello World" };
        var response = await _repository.CreateMessage("Asger", message, 1);
        expected = Response.NoContent;

        Assert.Equal(expected, response);

        var latest = await _repository.GetAsync();
        
        Assert.Equal(1, latest.latest);
    }

    public async ValueTask DisposeAsync() {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
