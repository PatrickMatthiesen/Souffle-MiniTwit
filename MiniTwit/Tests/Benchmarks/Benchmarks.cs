using BenchmarkDotNet.Attributes;
using MiniTwit.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.Extensions.Options;
using Microsoft.Data.Sqlite;
using MiniTwit.Infrastructure.Repositories;
using MiniTwit.Infrastructure.Models;

namespace Benchmarks;

[MemoryDiagnoser]
public class Benchmarks
{
    private ApplicationDbContext _context;
    private MessageRepository _messageRepository;
    private SimRepository _simRepository;
    private UserRepository _userRepository;

    [Params(10, 1_000, 100_000)]
    public int N;

    [GlobalSetup]
    public void Setup()
    {

        //setup a db context
        var _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseSqlite(_connection);

        OperationalStoreOptions storeOptions = new OperationalStoreOptions { };
        IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);

        var context = new ApplicationDbContext(builder.Options, operationalStoreOptions);
        context.Database.EnsureCreated();
        _context = context;

        var users = new List<ApplicationUser>();

        foreach (var userNumber in Enumerable.Range(0, N))
        {
            users.Add(new ApplicationUser
            {
                UserName = userNumber.ToString(),
                Email = $"{userNumber}@t.com"
            });
        }

        users[0].Follows = users.Where(u => u != users[0]).ToList();

        _context.Users.AddRange(users);

        _context.SaveChanges();

        _messageRepository = new MessageRepository(context);
        _simRepository = new SimRepository(context);
        _userRepository = new UserRepository(context);
    }

    //[Benchmark]
    public void Read_all_messages()
    {
        _messageRepository.ReadAll();
    }

    [Benchmark]
    public void Get_Follows()
    {
        _ = _context.Users.Include(u => u.Follows).FirstOrDefaultAsync(u => u.UserName == "0").Result.Follows;
    }

    [Benchmark]
    public void Get_Follows_User()
    {
        _userRepository.ReadFollowsAsync("0");
    }

    [Benchmark]
    public void Get_Follows_Sim()
    {
        _simRepository.GetFollows("0");
    }

    //[Benchmark]
    public void Read_user()
    {
        _userRepository.FindAsync("0");
    }
}
