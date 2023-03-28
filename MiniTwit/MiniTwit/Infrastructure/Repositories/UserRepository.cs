using Microsoft.EntityFrameworkCore;
using MiniTwit.Infrastructure.DbContext;
using MiniTwit.Infrastructure.Models;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;
using Prometheus;

namespace MiniTwit.Infrastructure.Repositories;
public class UserRepository : IUserRepository {
    private readonly ApplicationDbContext _context;

    // Prometheus counter metric
    private static Counter _usrCounter = Metrics
        .CreateCounter("total_users", "Number of users in DB.");

    public UserRepository(ApplicationDbContext context) {
        _context = context;
        _usrCounter.IncTo(_context.Users.Count());
    }

    public async Task<(Response Response, string userId)> CreateAsync(UserCreateDTO user) {
        var entity = await _context.Users.FindAsync(user.Id);
        Response response;

        if (entity is null)
        {
            entity = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Email,
                Messages = new List<Message>(),
                Follows = new List<ApplicationUser>()
            };

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            _usrCounter.Inc(); // Prometheus metric
            response = Response.Created;
        }
        else
        {
            response = Response.Conflict;
        }
        return (response, entity.Id);

    }


    public async Task<(Response Response, UserDTO)> FindAsync(string userId) {
        var entity = await _context.Users.FindAsync(userId);
        Response response;

        if (entity is null)
        {
            response = Response.NotFound;
            return (response, null);
        }
        else
        {
            response = Response.OK;
            return (response, new UserDTO(userId, entity.UserName, entity.Email));
        }
    }


    public async Task<Response> UpdateAsync(UserUpdateDTO user) {
        var entity = await _context.Users.FindAsync(user.Id);

        if (entity is null)
        {
            return Response.NotFound;
        }
        else
        {
            entity.Id = user.Id;
            entity.UserName = user.Name;
            entity.Email = user.Email;

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return Response.Updated;
        }
    }

    public async Task<Response> Follow(string userId, string targetName) {
        var entity = await _context.Users.Include("Follows").FirstOrDefaultAsync(u => u.Id == userId);

        if (entity is null)
            return Response.NotFound;
        if (entity.UserName == targetName)
            return Response.Conflict;

        var target = await _context.Users.FirstOrDefaultAsync(u => u.UserName == targetName);

        if (target is null)
            return Response.NotFound;

        if (entity.Follows.Contains(target))
        {
            return Response.Conflict;
        }

        entity.Follows.Add(target);
        await _context.SaveChangesAsync();
        return Response.NoContent;
    }

    public async Task<Response> UnFollowAsync(string userId, string targetName) {
        var entity = await _context.Users.Include("Follows").FirstOrDefaultAsync(u => u.Id == userId);

        if (entity is null)
            return Response.NotFound;
        if (entity.UserName == targetName)
            return Response.Conflict;

        var target = entity.Follows.FirstOrDefault(f => f.UserName == targetName);

        if (target is null)
            return Response.NotFound;

        if (!entity.Follows.Contains(target))
        {
            return Response.NotFound;
        }

        entity.Follows.Remove(target);
        await _context.SaveChangesAsync();
        return Response.NoContent;
    }

    public async Task<List<UserDTO>> ReadFollowsAsync(string Id) {
        var entity = await _context.Users.FindAsync(Id);

        var returnList = new List<UserDTO>();

        foreach (var f in entity.Follows)
        {
            returnList.Add(new UserDTO(f.Id, f.UserName, f.Email));
        }

        return returnList;
    }

    public async Task<List<UserDTO>> ReadFollowsAsyncQuery(string Id) {
        var entity = await _context.Users.FindAsync(Id);

        return
            (from u in _context.Users
             where u.Id == Id
             select new UserDTO(u.Id, u.UserName, u.Email)).ToList();

    }

    public async Task<List<MessageDTO>> ReadMessagesFromUserNameAsync(string userName) {
        var entity = await _context.Users.Include("Messages").FirstOrDefaultAsync(u => u.UserName == userName);

        var messages = new List<MessageDTO>();

        foreach (var m in entity.Messages)
        {
            messages.Add(new MessageDTO { Text = m.Text, PubDate = m.PubDate, AuthorName = m.Author.UserName });
        }

        return messages;
    }

    public async Task<List<MessageDTO>> ReadMyTimelineAsync(string id) {
        var user = await _context.Users.Include("Messages").Include("Follows.Messages").FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return null;

        var messages = from u in user.Follows
                       from m in u.Messages
                       select new MessageDTO { Text = m.Text, PubDate = m.PubDate, AuthorName = m.Author.UserName };

        return user.Messages.Select(m => new MessageDTO { Text = m.Text, PubDate = m.PubDate, AuthorName = m.Author.UserName }).Concat(messages).OrderByDescending(m => m.PubDate).Take(100).Reverse().ToList();
    }



    public async Task<Response> DeleteAsync(string userId, bool force = false) {
        var entity = await _context.Users.FindAsync(userId);

        Response response;

        if (entity is null)
        {
            response = Response.NotFound;
        }
        else
        {
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            response = Response.Deleted;
        }

        return response;
    }
}