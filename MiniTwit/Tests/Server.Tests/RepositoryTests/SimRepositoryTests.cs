using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using MiniTwit.Infrastructure.DbContext;
using MiniTwit.Infrastructure.Repositories;
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
        var actual = await _repository.GetLatestAsync();

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task RegisterUser_And_SendMessage_Then_CheckLatest() {
        var user = new SimUserDTO { userName = "Asger" };
        var expected = Response.NoContent;
        var actual = await _repository.RegisterUser(user, 0);

        Assert.Equal(expected, actual);

        var message = new SimMessageDTO { content = "Hello World" };
        var response = await _repository.CreateMessage("Asger", message, 1);
        expected = Response.NoContent;

        Assert.Equal(expected, response);

        var latest = await _repository.GetLatestAsync();

        Assert.Equal(1, latest.latest);
    }

    // test that adds three users and have them follow each other
    [Fact]
    public async Task RegisterUsers_And_Follow_Then_CheckFollows()
    {
        var expected = Response.NoContent;
        var user1 = new SimUserDTO { userName = "Asger" };
        var user2 = new SimUserDTO { userName = "Kure" };
        var user3 = new SimUserDTO { userName = "Rasmus" };
        var actual = await _repository.RegisterUser(user1, 0);
        Assert.Equal(expected, actual);
        actual = await _repository.RegisterUser(user2, 1);
        Assert.Equal(expected, actual);
        actual = await _repository.RegisterUser(user3, 2);
        Assert.Equal(expected, actual);

        var response = await _repository.CreateOrRemoveFollower(user1.userName, user2.userName, null, true);
        Assert.Equal(expected, response);
        response = await _repository.CreateOrRemoveFollower(user1.userName, user3.userName, null, true);
        Assert.Equal(expected, response);
        response = await _repository.CreateOrRemoveFollower(user2.userName, user1.userName, null, true);
        Assert.Equal(expected, response);
        response = await _repository.CreateOrRemoveFollower(user2.userName, user3.userName, null, true); 
        Assert.Equal(expected, response);
        response = await _repository.CreateOrRemoveFollower(user3.userName, user1.userName, null, true);
        Assert.Equal(expected, response);
        response = await _repository.CreateOrRemoveFollower(user3.userName, user2.userName, null, true);
        Assert.Equal(expected, response);
        
        var follows = await _repository.GetFollows("Asger");
        Assert.Equal(2, follows.Count);
        Assert.Equal(user2.userName, follows[0].Name);
        Assert.Equal(user3.userName, follows[1].Name);
        follows = await _repository.GetFollows("Rasmus");
        Assert.Equal(2, follows.Count);
        follows = await _repository.GetFollows("Kure");
        Assert.Equal(2, follows.Count);
    }


    public async ValueTask DisposeAsync() {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
