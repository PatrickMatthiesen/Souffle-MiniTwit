namespace Server.Tests.RepositoryTests;

using Duende.IdentityServer.EntityFramework.Options;
using MiniTwit.Infrastructure.Models;
using Microsoft.Extensions.Options;
using MiniTwit.Infrastructure.DbContext;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Shared.DTO;

public sealed class UserRepositoryTests : IAsyncDisposable {
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests() {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlite(_connection);

        OperationalStoreOptions storeOptions = new OperationalStoreOptions { };
        IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

        var context = new ApplicationDbContext(builder.Options, operationalStoreOptions);
        context.Database.EnsureCreated();

        var Asger = new ApplicationUser { UserName = "Asger", Id = "0", Email = "Asger@itu.dk" };
        var Kure = new ApplicationUser { UserName = "Kure", Id = "1", Email = "Kure@itu.dk" };
        var Smus = new ApplicationUser { UserName = "Smus", Id = "2", Email = "Smus@itu.dk" };

        context.Users.AddRange(Asger, Kure, Smus);

        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_Succes() {
        var expected = new UserCreateDTO("5", "Patrick", "Patrick@itu.dk");
        var (response, userId) = await _repository.CreateAsync(expected);

        var (findResponse, entity) = await _repository.FindAsync(userId);

        Assert.Equal(MiniTwit.Shared.Response.Created, response);
        Assert.Equal(entity.Id, userId);
    }

    [Fact]
    public async Task CreateAsync_Conflict() {
        var expected1 = new UserCreateDTO("0", "Asger", "Asger@itu.dk");

        var (response1, userId1) = await _repository.CreateAsync(expected1);

        Assert.Equal(MiniTwit.Shared.Response.Conflict, response1);
    }


    [Fact]
    public async Task FindAsync_NotFound() {
        var (result, UserDTO) = await _repository.FindAsync("42");
        Assert.Equal(MiniTwit.Shared.Response.NotFound, result);
    }

    [Fact]
    public async Task FindAsync_OK() {
        var expected = new UserDTO("1", "Kure", "Kure@itu.dk");
        var (result, UserDTO) = await _repository.FindAsync("1");

        Assert.Equal(MiniTwit.Shared.Response.OK, result);
        Assert.Equal(expected, UserDTO);
    }

    [Fact]
    public async Task UpdateAsync_Updated() {
        var UpdateDTO = new UserUpdateDTO("1", "KureSmure", "KureSmure@itu.dk");

        var result = await _repository.UpdateAsync(UpdateDTO);

        Assert.Equal(MiniTwit.Shared.Response.Updated, result);
    }

    [Fact]
    public async Task UpdateAsync_NotFound() {
        var UpdateDTO = new UserUpdateDTO("42", "Patrick", "Patrick@itu.dk");

        var result = await _repository.UpdateAsync(UpdateDTO);

        Assert.Equal(MiniTwit.Shared.Response.NotFound, result);
    }

    [Fact]
    public async Task Follow_Returns_NoContent() {
        var expectedFollower = new UserCreateDTO("5", "Patrick", "Patrick@itu.dk");
        var (response1, followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO("10", "Søren", "Søren@itu.dk");
        var (response2, targetId) = await _repository.CreateAsync(targetFollower);

        var response = await _repository.Follow(followerId, targetFollower.Name);

        Assert.Equal(MiniTwit.Shared.Response.NoContent, response);
    }

    [Fact]
    public async Task Follow_Conflict() {
        var expectedFollower = new UserCreateDTO("5", "Patrick", "Patrick@itu.dk");
        var (response1, followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO("10", "Søren", "Søren@itu.dk");
        var (response2, targetId) = await _repository.CreateAsync(targetFollower);

        var OK = await _repository.Follow(followerId, targetFollower.Name);
        var Conflict = await _repository.Follow(followerId, targetFollower.Name);

        Assert.Equal(MiniTwit.Shared.Response.Conflict, Conflict);
    }

    [Fact]
    public async Task ReadFollowsAsync() {
        var expectedFollower = new UserCreateDTO("5", "Patrick", "Patrick@itu.dk");
        var (response1, followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO("10", "Søren", "Søren@itu.dk");
        var (response2, targetId) = await _repository.CreateAsync(targetFollower);

        var tmp = await _repository.Follow(followerId, targetFollower.Name);

        var result = await _repository.ReadFollowsAsync(followerId);

        var expectedList = new List<UserDTO> { new UserDTO(targetId, "Søren", "Søren@itu.dk") };

        Assert.Equal(result, expectedList);
    }

    [Fact]
    public async Task Unfollow() {
        var expectedFollower = new UserCreateDTO("5", "Patrick", "Patrick@itu.dk");
        var (response1, followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO("10", "Søren", "Søren@itu.dk");
        var (response2, targetId) = await _repository.CreateAsync(targetFollower);

        var followResponse = await _repository.Follow(followerId, targetId);
        var unfollowResponse = await _repository.UnFollow(followerId, targetFollower.Name);

        var result = await _repository.ReadFollowsAsync(followerId);
        var expectedList = new List<UserDTO> { };

        Assert.Equal(result, expectedList);
    }

    /*
        The following methods is outcommented due to an error with the how messages interacts with the user -
        method might be redundant
    */

    // [Fact]
    // public async Task ReadMessagesFromUserIdAsync()
    // {   

    //     var userDTO = new UserCreateDTO ("5","Patrick","Patrick@itu.dk");
    //     var (userResponse , userId) = await _repository.CreateAsync(userDTO);

    //     var user = new ApplicationUser{UserName = "Patrick"};

    //     var message1 = new Message {Author = user, Id = 0, Text = "Patrick says hello"};

    //     _context.Messages.Add(message1);

    //    var Messages = await _repository.ReadMessagesFromUserIdAsync(userId);

    //    Assert.Equal(1, Messages.Count());

    // }

    [Fact]
    public async Task DeleteAsync_Succes() {
        var expected = new UserCreateDTO("5", "Patrick", "Patrick@itu.dk");
        var (response, userId) = await _repository.CreateAsync(expected);

        var (findResponse, entity) = await _repository.FindAsync(userId);

        Assert.Equal(MiniTwit.Shared.Response.Created, response);
        Assert.Equal(entity.Id, userId);

        var deletedResponse = _repository.DeleteAsync(userId);
        Assert.Equal(MiniTwit.Shared.Response.Deleted, deletedResponse.Result);

    }

    [Fact]
    public async Task DeleteAsync_NotFound() {
        var deletedResponse = await _repository.DeleteAsync("42");
        Assert.Equal(MiniTwit.Shared.Response.NotFound, deletedResponse);

    }

    public async ValueTask DisposeAsync() {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

}

