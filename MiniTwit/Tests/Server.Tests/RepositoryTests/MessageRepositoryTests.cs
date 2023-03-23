using Duende.IdentityServer.EntityFramework.Options;
using MiniTwit.Infrastructure.Models;
using Microsoft.Extensions.Options;
using MiniTwit.Infrastructure.DbContext;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Shared.DTO;

namespace Server.Tests.RepositoryTests;
public sealed class MessageRepositoryTests : IAsyncDisposable {
    private readonly SqliteConnection _connection;
    private readonly ApplicationDbContext _context;
    private readonly MessageRepository _repository;

    public MessageRepositoryTests() {
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

        var msg1 = new Message { Text = "Hello from Asger", Author = Asger };
        var msg2 = new Message { Text = "Hi Asger", Author = Kure };
        var msg3 = new Message { Text = "Skrrrt", Author = Smus };

        context.Users.AddRange(Asger, Kure, Smus);
        context.Messages.AddRange(msg1, msg2, msg3);

        context.SaveChanges();

        _context = context;
        _repository = new MessageRepository(_context);
    }


    [Fact]
    public async Task ReadAll() {

        var msg1 = new MessageDTO { AuthorName = "Asger", Text = "Hello from Asger" };
        var msg2 = new MessageDTO { AuthorName = "Kure", Text = "Hi Asger" };
        var msg3 = new MessageDTO { AuthorName = "Smus", Text = "Skrrrt" };
        var resultList = new List<MessageDTO> { msg1, msg2, msg3 };

        var messages = await _repository.ReadAll();
        Assert.Equal(resultList, messages);

    }

    /*Below test doesn't pass and needs to be fixed*/
    [Fact]
    public async Task AddMessage_succes() {

        var createDTO = new CreateMessageDTO { Text = "Hello from Asger", AuthorId = "0" };

        var msg = await _repository.AddMessage(createDTO);
        Assert.True(msg.IsSome);
        var test = await _repository.ReadAsync(msg.Value.Id);
        var result = new MessageDTO { AuthorName = "Asger", Text = "Hello from Asger" };
        Assert.True(test.IsSome);

    }

    //test time conversion from utc +000 to local
    [Fact]
    public void TimeConversion()
    {
        // Assume we have a UTC time
        DateTime utcTime = new DateTime(2000, 6, 1);
        DateTime server = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        Assert.Equal(utcTime, server);

        TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
        DateTime utcPlusOneDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        Assert.Equal(utcPlusOneDateTime.ToString(), server.ToLocalTime().ToString());

        var timeString = TimeSinceOrDate(server, utcPlusOneDateTime);
        Assert.Equal($"{utcPlusOneDateTime.Second} sec.", timeString);


        timeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
        DateTime utcMinusSix = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        Assert.Equal(utcMinusSix.ToString(), server.ToString());

        timeString = TimeSinceOrDate(server, utcMinusSix);
        Assert.Equal($"{utcMinusSix.Second} sec.", timeString);
    }

    public string TimeSinceOrDate(DateTime dateTime, DateTime now)
    {
        TimeSpan ts = now - dateTime.ToLocalTime();

        return ts switch
        {
            _ when ts.TotalDays >= 7 => dateTime.ToString("d MMM yyyy"),
            _ when ts.TotalHours >= 24 => $"{(int)Math.Floor(ts.TotalDays)} d.",
            _ when ts.TotalMinutes >= 60 => $"{(int)Math.Floor(ts.TotalHours)} h.",
            _ when ts.TotalSeconds >= 60 => $"{(int)Math.Floor(ts.TotalMinutes)} min.",
            _ => $"{(int)Math.Floor(ts.TotalSeconds)} sec."
        };
    }

    // [Fact]
    // public async Task ReadAsync_succes()
    // {   

    //     var msg1 = new MessageDTO{AuthorName = "Asger", Text = "Hello from Asger", Id=5};
    //     var msg2 = new MessageDTO{AuthorName = "Kure", Text = "Hi Asger"};
    //     var msg3 = new MessageDTO{AuthorName = "Smus", Text = "Skrrrt"};
    //     var resultList = new List<MessageDTO>{msg1,msg2,msg3};

    //     var message = await _repository.ReadAsync(5);
    //     Assert.Equal(true,message.IsSome);

    // }




    public async ValueTask DisposeAsync() {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

}
