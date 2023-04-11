using Duende.IdentityServer.EntityFramework.Options;
using MiniTwit.Infrastructure.Models;
using Microsoft.Extensions.Options;
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

        var msg1 = new Message { Text = "Hello from Asger", Author = Asger, PubDate = new DateTime(1, 1, 1)};
        var msg2 = new Message { Text = "Hi Asger", Author = Kure, PubDate = new DateTime(1, 1, 2)};
        var msg3 = new Message { Text = "Skrrrt", Author = Smus, PubDate = new DateTime(1, 1, 3) };
        var msg4 = new Message { Text = "Message 4", Author = Smus, PubDate = new DateTime(1, 1, 4)};
        var msg5 = new Message { Text = "Message 5", Author = Smus, PubDate = new DateTime(1, 1, 5)};
        var msg6 = new Message { Text = "Message 6", Author = Smus, PubDate = new DateTime(1, 1, 6)};
        var msg7 = new Message { Text = "Message 7", Author = Smus, PubDate = new DateTime(1, 1, 7)};
        var msg8 = new Message { Text = "Message 8", Author = Smus, PubDate = new DateTime(1, 1, 8)};
        var msg9 = new Message { Text = "Message 9", Author = Smus, PubDate = new DateTime(1, 1, 9)};
        var msg10 = new Message { Text = "Message 10", Author = Smus, PubDate = new DateTime(1, 1, 10)};
        var msg11 = new Message { Text = "Message 11", Author = Smus, PubDate = new DateTime(1, 1, 11)};
        var msg12 = new Message { Text = "Message 12", Author = Smus, PubDate = new DateTime(1, 1, 12)};
        var msg13 = new Message { Text = "Message 13", Author = Smus, PubDate = new DateTime(1, 1, 13)};
        var msg14 = new Message { Text = "Message 14", Author = Smus, PubDate = new DateTime(1, 1, 14)};
        var msg15 = new Message { Text = "Message 15", Author = Smus, PubDate = new DateTime(1, 1, 15)};
        var msg16 = new Message { Text = "Message 16", Author = Smus, PubDate = new DateTime(1, 1, 16)};
        var msg17 = new Message { Text = "Message 17", Author = Smus, PubDate = new DateTime(1, 1, 17)};
        var msg18 = new Message { Text = "Message 18", Author = Smus, PubDate = new DateTime(1, 1, 18)};
        var msg19 = new Message { Text = "Message 19", Author = Smus, PubDate = new DateTime(1, 1, 19)};
        var msg20 = new Message { Text = "Message 20", Author = Smus, PubDate = new DateTime(1, 1, 20)};
        var msg21 = new Message { Text = "Message 21", Author = Smus, PubDate = new DateTime(1, 1, 21)};

        context.Users.AddRange(Asger, Kure, Smus);
        context.Messages.AddRange(msg1, msg2, msg3, msg4, 
            msg5, msg6, msg7, msg8, msg9, msg10, msg11, msg12,msg13,
            msg14,msg15,msg16,msg17,msg18,msg19,msg20,msg21);

        context.SaveChanges();

        _context = context;
        _repository = new MessageRepository(_context);
    }


    [Fact]
    public async Task ReadAll() {
        var expected = new List<MessageDTO> {
            new MessageDTO { Text = "Hello from Asger", AuthorName = "Asger", PubDate = new DateTime(1, 1, 1) },
            new MessageDTO { Text = "Hi Asger", AuthorName = "Kure", PubDate = new DateTime(1, 1, 2) },
            new MessageDTO { Text = "Skrrrt", AuthorName = "Smus", PubDate = new DateTime(1, 1, 3) },
            new MessageDTO { Text = "Message 4", AuthorName = "Smus", PubDate = new DateTime(1, 1, 4) },
            new MessageDTO { Text = "Message 5", AuthorName = "Smus", PubDate = new DateTime(1, 1, 5) },
            new MessageDTO { Text = "Message 6", AuthorName = "Smus", PubDate = new DateTime(1, 1, 6) },
            new MessageDTO { Text = "Message 7", AuthorName = "Smus", PubDate = new DateTime(1, 1, 7) },
            new MessageDTO { Text = "Message 8", AuthorName = "Smus", PubDate = new DateTime(1, 1, 8) },
            new MessageDTO { Text = "Message 9", AuthorName = "Smus", PubDate = new DateTime(1, 1, 9) },
            new MessageDTO { Text = "Message 10", AuthorName = "Smus", PubDate = new DateTime(1, 1, 10) },
            new MessageDTO { Text = "Message 11", AuthorName = "Smus", PubDate = new DateTime(1, 1, 11) },
            new MessageDTO { Text = "Message 12", AuthorName = "Smus", PubDate = new DateTime(1, 1, 12) },
            new MessageDTO { Text = "Message 13", AuthorName = "Smus", PubDate = new DateTime(1, 1, 13) },
            new MessageDTO { Text = "Message 14", AuthorName = "Smus", PubDate = new DateTime(1, 1, 14) },
            new MessageDTO { Text = "Message 15", AuthorName = "Smus", PubDate = new DateTime(1, 1, 15) },
            new MessageDTO { Text = "Message 16", AuthorName = "Smus", PubDate = new DateTime(1, 1, 16) },
            new MessageDTO { Text = "Message 17", AuthorName = "Smus", PubDate = new DateTime(1, 1, 17) },
            new MessageDTO { Text = "Message 18", AuthorName = "Smus", PubDate = new DateTime(1, 1, 18) },
            new MessageDTO { Text = "Message 19", AuthorName = "Smus", PubDate = new DateTime(1, 1, 19) },
            new MessageDTO { Text = "Message 20", AuthorName = "Smus", PubDate = new DateTime(1, 1, 20) },
            new MessageDTO { Text = "Message 21", AuthorName = "Smus", PubDate = new DateTime(1, 1, 21) }
        };

        var messages = await _repository.ReadAll();
        Assert.Equal(expected, messages);

    }
    
    [Fact]
    public async Task ReadAllByPage_Page1_Size8() {
        var expected = new List<MessageDTO> {
            new MessageDTO { Text = "Message 14", AuthorName = "Smus", PubDate = new DateTime(1, 1, 14) },
            new MessageDTO { Text = "Message 15", AuthorName = "Smus", PubDate = new DateTime(1, 1, 15) },
            new MessageDTO { Text = "Message 16", AuthorName = "Smus", PubDate = new DateTime(1, 1, 16) },
            new MessageDTO { Text = "Message 17", AuthorName = "Smus", PubDate = new DateTime(1, 1, 17) },
            new MessageDTO { Text = "Message 18", AuthorName = "Smus", PubDate = new DateTime(1, 1, 18) },
            new MessageDTO { Text = "Message 19", AuthorName = "Smus", PubDate = new DateTime(1, 1, 19) },
            new MessageDTO { Text = "Message 20", AuthorName = "Smus", PubDate = new DateTime(1, 1, 20) },
            new MessageDTO { Text = "Message 21", AuthorName = "Smus", PubDate = new DateTime(1, 1, 21) },
        };

        var actual = await _repository.ReadAllByPage(1, 8);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task ReadAllByPage_Page1_Size4() {
        var expected = new List<MessageDTO> {
            new MessageDTO { Text = "Message 18", AuthorName = "Smus", PubDate = new DateTime(1, 1, 18) },
            new MessageDTO { Text = "Message 19", AuthorName = "Smus", PubDate = new DateTime(1, 1, 19) },
            new MessageDTO { Text = "Message 20", AuthorName = "Smus", PubDate = new DateTime(1, 1, 20) },
            new MessageDTO { Text = "Message 21", AuthorName = "Smus", PubDate = new DateTime(1, 1, 21) },
        };

        var actual = await _repository.ReadAllByPage(1, 4);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task ReadAllByPage_Page2_Size4() {
        var expected = new List<MessageDTO> {
            new MessageDTO { Text = "Message 14", AuthorName = "Smus", PubDate = new DateTime(1, 1, 14) },
            new MessageDTO { Text = "Message 15", AuthorName = "Smus", PubDate = new DateTime(1, 1, 15) },
            new MessageDTO { Text = "Message 16", AuthorName = "Smus", PubDate = new DateTime(1, 1, 16) },
            new MessageDTO { Text = "Message 17", AuthorName = "Smus", PubDate = new DateTime(1, 1, 17) },
        };

        var actual = await _repository.ReadAllByPage(2, 4);
        Assert.Equal(expected, actual);
    }
    
    [Fact]
    public async Task ReadAllByPage_Page1_Size16() {
        var expected = new List<MessageDTO> {
            new MessageDTO { Text = "Message 6", AuthorName = "Smus", PubDate = new DateTime(1, 1, 6) },
            new MessageDTO { Text = "Message 7", AuthorName = "Smus", PubDate = new DateTime(1, 1, 7) },
            new MessageDTO { Text = "Message 8", AuthorName = "Smus", PubDate = new DateTime(1, 1, 8) },
            new MessageDTO { Text = "Message 9", AuthorName = "Smus", PubDate = new DateTime(1, 1, 9) },
            new MessageDTO { Text = "Message 10", AuthorName = "Smus", PubDate = new DateTime(1, 1, 10) },
            new MessageDTO { Text = "Message 11", AuthorName = "Smus", PubDate = new DateTime(1, 1, 11) },
            new MessageDTO { Text = "Message 12", AuthorName = "Smus", PubDate = new DateTime(1, 1, 12) },
            new MessageDTO { Text = "Message 13", AuthorName = "Smus", PubDate = new DateTime(1, 1, 13) },
            new MessageDTO { Text = "Message 14", AuthorName = "Smus", PubDate = new DateTime(1, 1, 14) },
            new MessageDTO { Text = "Message 15", AuthorName = "Smus", PubDate = new DateTime(1, 1, 15) },
            new MessageDTO { Text = "Message 16", AuthorName = "Smus", PubDate = new DateTime(1, 1, 16) },
            new MessageDTO { Text = "Message 17", AuthorName = "Smus", PubDate = new DateTime(1, 1, 17) },
            new MessageDTO { Text = "Message 18", AuthorName = "Smus", PubDate = new DateTime(1, 1, 18) },
            new MessageDTO { Text = "Message 19", AuthorName = "Smus", PubDate = new DateTime(1, 1, 19) },
            new MessageDTO { Text = "Message 20", AuthorName = "Smus", PubDate = new DateTime(1, 1, 20) },
            new MessageDTO { Text = "Message 21", AuthorName = "Smus", PubDate = new DateTime(1, 1, 21) }
        };

        var actual = await _repository.ReadAllByPage(1, 16);
        Assert.Equal(expected, actual);
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

    /// <summary>
    /// test time conversion from utc+0 to local
    /// </summary>
    [Fact]
    public void TimeConversion()
    {
        // Assume we have a UTC time
        DateTime utcTime = new DateTime(2000, 6, 1);
        DateTime server = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
        Assert.Equal(utcTime, server);

        TimeZoneInfo timeZone = TimeZoneInfo.Local;
        DateTime utcPlusOneDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
        Assert.Equal(utcPlusOneDateTime.ToString(), server.ToLocalTime().ToString());

        var timeString = TimeSinceOrDate(server, utcPlusOneDateTime);
        Assert.Equal($"{utcPlusOneDateTime.Second} sec.", timeString);
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
