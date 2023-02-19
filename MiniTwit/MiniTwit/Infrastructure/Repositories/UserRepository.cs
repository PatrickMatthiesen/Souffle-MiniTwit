using Infrastructure.Data;
using Infrastructure.Models;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace MiniTwit.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(Response Response, string UserId)> CreateAsync(UserCreateDTO user)
    {
        var entity = await _context.Users.FindAsync(user);

        Response response;

        if (entity is null)
        {
            entity = new ApplicationUser
            {
                UserName = user.Name,
                Email = user.Email
            };

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            response = Response.Created;
        }
        else
        {
            response = Response.Conflict;
        }
        return (response, entity.Id);

    }


    public async Task<UserDTO> FindAsync(string userId)
    {
        var entity = await _context.Users.FindAsync(userId);

        return new UserDTO(userId, entity.UserName, entity.Email);

    }

    public async Task<Response> UpdateAsync(UserUpdateDTO user)
    {
        var entity = await _context.Users.FindAsync(user);
        entity.Id = user.Id;
        entity.UserName = user.Name;
        entity.Email = user.Email;

        if (entity is null)
        {
            return Response.NotFound;
        }
        else
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return Response.Updated;
        }
    }

    public async Task<List<UserDTO>> ReadFollowsAsync(string Id)
    {
        var entity = await _context.Users.FindAsync(Id);

        var returnList = new List<UserDTO>();

        foreach (var f in entity.Follows)
        {
            returnList.Add(new UserDTO(f.Id, f.UserName, f.Email));
        }

        return returnList;
    }

    public async Task<Response> UnFollow(string Id_Own, string Id_Target)
    {
        var entity = await _context.Users.FindAsync(Id_Own);

        var target = entity.Follows.FirstOrDefault(f => f.Id == Id_Target);

        Response response;

        if (target is not null)
        {
            entity.Follows.Remove(target);
            await _context.SaveChangesAsync();
            response = Response.Updated;
        }
        else
        {
            response = Response.NotFound;
        }
        return response;
    }

    public async Task<Response> Follow(string Id_Own, string Id_Target)
    {
        var entity = await _context.Users.FindAsync(Id_Own);

        var target = entity.Follows.FirstOrDefault(f => f.Id == Id_Target);

        Response response;
        if (target is null) { }

        if (entity.Follows.Contains(target))
        {
            response = Response.Conflict;
        }
        else
        {
            entity.Follows.Add(target);
            await _context.SaveChangesAsync();
            response = Response.Updated;
        }
        return response;
    }


    public async Task<List<MessageDTO>> ReadMessagesFromUserIdAsync(string Id)
    {
        var entity = await _context.Users.FindAsync(Id);

        var messages = new List<MessageDTO>();

        foreach (var m in entity.Messages)
        {
            messages.Add(new MessageDTO { Text = m.Text, PubDate = m.PubDate, AuthorId = Id, Flagged = m.Flagged });
        }

        return messages;
    }


    public async Task<Response> DeleteAsync(string userId, bool force = false)
    {
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
