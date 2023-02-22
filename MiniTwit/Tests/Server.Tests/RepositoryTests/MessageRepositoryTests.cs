namespace Server.Tests;

using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using Moq;

public sealed class MessageRepositoryTests : IAsyncDisposable
{
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly MessageRepository _repository;

    public MessageRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlite(_connection);

        OperationalStoreOptions storeOptions = new OperationalStoreOptions {};
        IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

        var context = new ApplicationDbContext(builder.Options, operationalStoreOptions);
        context.Database.EnsureCreated();

        // var Asger = new ApplicationUser{ UserName = "Asger", Id = "0", Email="Asger@itu.dk"};
        // var Kure = new ApplicationUser{ UserName = "Kure", Id = "1", Email="Kure@itu.dk"};
        // var Smus = new ApplicationUser{ UserName = "Smus", Id = "2", Email="Smus@itu.dk"};

        // context.Users.AddRange(Asger, Kure, Smus);
        
        context.SaveChanges();

        _context = context;
        _repository = new MessageRepository(_context);
    }

     public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

}
