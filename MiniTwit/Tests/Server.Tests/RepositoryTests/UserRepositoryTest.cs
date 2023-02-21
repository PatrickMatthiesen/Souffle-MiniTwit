namespace Server.Tests;

using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure.Models;
using Microsoft.Extensions.Options;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Shared.DTO;

public sealed class UserRepositoryTests : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlite(_connection);

        OperationalStoreOptions storeOptions = new OperationalStoreOptions {};
        IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

        var context = new ApplicationDbContext(builder.Options, operationalStoreOptions);
        context.Database.EnsureCreated();

        var Asger = new ApplicationUser{ UserName = "Asger", Id = "0", Email="Asger@itu.dk"};
        var Kure = new ApplicationUser{ UserName = "Kure", Id = "1", Email="Kure@itu.dk"};
        var Smus = new ApplicationUser{ UserName = "Smus", Id = "2", Email="Smus@itu.dk"};

        var message1 = new Message {Author = Asger, Id = 0, Text = "Asger says hello"};
        //var message2 = new Message {Author = Smus, Id = 1, Text = "Hi Asger!"};

        context.Users.AddRange(Asger, Kure, Smus);
        context.Messages.AddRange(message1);
        //context.Messages.AddRange(message1, message2);
        
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_Succes()
    {
        var expected = new UserCreateDTO ("5","Patrik","Patrik@itu.dk");
        var (response , userId) = await _repository.CreateAsync(expected);
        
        var (findResponse, entity) = await _repository.FindAsync(userId);

        Assert.Equal(MiniTwit.Shared.Response.Created, response);
        Assert.Equal(entity.Id, userId);
    }

    [Fact]
     public async Task CreateAsync_Conflict()
    {
        var expected1 = new UserCreateDTO ("0","Asger","Asger@itu.dk");
        
        var (response1 , userId1) = await _repository.CreateAsync(expected1);
        
        Assert.Equal(MiniTwit.Shared.Response.Conflict, response1);
    }


    [Fact]
    public async Task FindAsync_OK()
    {
        var (result, UserDTO) = await _repository.FindAsync("42");
        Assert.Equal(MiniTwit.Shared.Response.NotFound, result);
    }

    [Fact]
    public async Task FindAsync_NotFound()
    {   
        
        var expected = new UserDTO("1","Kure","Kure@itu.dk");
        var (result, UserDTO) = await _repository.FindAsync("1");
        
        Assert.Equal(MiniTwit.Shared.Response.OK, result);
        Assert.Equal(expected, UserDTO);
    }

    [Fact]
    public async Task UpdateAsync_Updated()
    {
        var UpdateDTO = new UserUpdateDTO("1","KureSmure","KureSmure@itu.dk");

        var result = await _repository.UpdateAsync(UpdateDTO);
        
        Assert.Equal(MiniTwit.Shared.Response.Updated, result);
    }

    [Fact]
    public async Task UpdateAsync_NotFound()
    {
        var UpdateDTO = new UserUpdateDTO("42","Patrik","Patrik@itu.dk");

        var result = await _repository.UpdateAsync(UpdateDTO);
        
        Assert.Equal(MiniTwit.Shared.Response.NotFound, result);
    }
   
    [Fact]
    public async Task Follow_OK()
    {
        var expectedFollower = new UserCreateDTO ("5","Patrik","Patrik@itu.dk");
        var (response1 , followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO ("10","Søren","Søren@itu.dk");
        var (response2 , targetId) = await _repository.CreateAsync(targetFollower);

        var response = await _repository.Follow(followerId, targetId);

        Assert.Equal(MiniTwit.Shared.Response.OK, response);
    }

    [Fact]
    public async Task Follow_Conflict()
    {
        var expectedFollower = new UserCreateDTO ("5","Patrik","Patrik@itu.dk");
        var (response1 , followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO ("10","Søren","Søren@itu.dk");
        var (response2 , targetId) = await _repository.CreateAsync(targetFollower);

        var OK = await _repository.Follow(followerId, targetId);
        var Conflict = await _repository.Follow(followerId, targetId);

        Assert.Equal(MiniTwit.Shared.Response.Conflict, Conflict);
    }

    [Fact]
    public async Task ReadFollowsAsync()
    {
        var expectedFollower = new UserCreateDTO ("5","Patrik","Patrik@itu.dk");
        var (response1 , followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO ("10","Søren","Søren@itu.dk");
        var (response2 , targetId) = await _repository.CreateAsync(targetFollower);

        var tmp = await _repository.Follow(followerId, targetId);

        var result = await _repository.ReadFollowsAsync(followerId);

        var expectedList = new List<UserDTO> { new UserDTO(targetId,"Søren","Søren@itu.dk") } ;

        Assert.Equal(result,expectedList);
    }

    [Fact]
    public async Task Unfollow()
    {
        var expectedFollower = new UserCreateDTO ("5","Patrik","Patrik@itu.dk");
        var (response1 , followerId) = await _repository.CreateAsync(expectedFollower);

        var targetFollower = new UserCreateDTO ("10","Søren","Søren@itu.dk");
        var (response2 , targetId) = await _repository.CreateAsync(targetFollower);

        var followResponse = await _repository.Follow(followerId, targetId);
        var unfollowResponse = await _repository.UnFollow(followerId, targetId);

        var result = await _repository.ReadFollowsAsync(followerId);
        var expectedList = new List<UserDTO> {} ;

        Assert.Equal(result,expectedList);
    }

    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

}

