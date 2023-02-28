using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace Infrastructure.Repositories;

public class SimRepository : ISimRepository
{
    private readonly ApplicationDbContext _context;

    public SimRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<List<MessageDTO>> GetMsgs()
    {
        return _context.Messages.Select(m => new MessageDTO
        {
            Text = m.Text,
            PubDate = m.PubDate,
            AuthorName = m.Author.UserName
        }).ToListAsync();
    }

    public Task<List<MessageDTO>> GetMsgsFromUserID(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Response> CreateMessage(string username, SimMessageDTO newMessage, int? latestMessage)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserDTO>> GetFollows()
    {
        throw new NotImplementedException();
    }

    public Task<Response> CreateFollower()
    {
        throw new NotImplementedException();
    }
    
    public async Task<Response> Follow(string username, string Id_Target)
    {
        var entity = await _context.Users.FindAsync(username);

        var target = _context.Users.FindAsync(Id_Target);
        var targetUser = target.Result;

        Response response;

        if (entity.Follows.Contains(targetUser))
        {
            response = Response.Conflict;
        }
        else
        {
            entity.Follows.Add(targetUser);
            await _context.SaveChangesAsync();
            response = Response.OK;
        }
        return response;
    }
}