using Microsoft.EntityFrameworkCore;
using MiniTwit.Infrastructure.DbContext;
using MiniTwit.Infrastructure.Models;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace MiniTwit.Infrastructure.Repositories;

public class SimRepository : ISimRepository {
    private readonly ApplicationDbContext _context;

    public SimRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<LatestDTO> GetLatestAsync() {
        var entity = await _context.Latests.FirstOrDefaultAsync();

        if (entity == null) {
            return new LatestDTO { };
        }

        return new LatestDTO() { latest = entity.latest };
    }

    public async Task<Response> RegisterUser(SimUserDTO user, int? latestMessage) {
        var entity = await _context.Users.FindAsync(user.userName);

        if (entity is null) {
            entity = new ApplicationUser {
                UserName = user.userName,
                Email = user.email,
                Messages = new List<Message>(),
                Follows = new List<ApplicationUser>()
            };

            await _context.AddAsync(entity);
            await UpdateLatest(latestMessage);
            await _context.SaveChangesAsync();
            
            return Response.NoContent;
        }

        return Response.Conflict;
    }





    public async Task<Response> UpdateAsync(LatestDTO dto) {
        var entity = await _context.Latests.FirstOrDefaultAsync();
        entity.latest = dto.latest;
        await _context.SaveChangesAsync();
        return Response.Created;
    }


    public Task<List<MessageDTO>> GetMsgs() {
        return _context.Messages.Select(m => new MessageDTO {
            Text = m.Text,
            PubDate = m.PubDate,
            AuthorName = m.Author.UserName
        }).ToListAsync();
    }

    public List<MessageDTO> GetMsgsFromUserID(string id) {
        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return Enumerable.Empty<MessageDTO>().ToList();
        return user.Messages.Select(m => new MessageDTO {
            Text = m.Text,
            PubDate = m.PubDate,
            AuthorName = m.Author.UserName
        }).ToList();
    }

    public async Task<Response> CreateMessage(string userName, SimMessageDTO newMessage, int? latestMessage) {
        var author = await _context.Users.Include("Messages").FirstOrDefaultAsync(u => u.UserName == userName);

        if (author == null) {
            return Response.NotFound;
        }

        author.Messages.Add(new Message {
            Text = newMessage.content,
            PubDate = DateTime.Now,
            Author = author,
            Flagged = 0 //TODO
        });

        await UpdateLatest(latestMessage);

        await _context.SaveChangesAsync();
        return Response.NoContent;
    }

    public async Task<List<UserDTO>> GetFollows(string username) {
        var entity = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

        var returnList = new List<UserDTO>();

        foreach (var f in entity.Follows) {
            returnList.Add(new UserDTO(f.Id, f.UserName, f.Email));
        }

        return returnList;
    }


    public async Task<Response> CreateOrRemoveFollower(string username, string Id_Target, int? latestMessage, bool follow = true) {
        var entity = await _context.Users.Include("Follows").FirstOrDefaultAsync(u => u.UserName == username);

        if (entity == null) {
            return Response.NotFound;
        }

        var targetUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == Id_Target);

        if (targetUser == null) {
            return Response.NotFound;
        }

        if (follow)
        {
            entity.Follows.Add(targetUser);
        }
        else
        {
            entity.Follows.Remove(targetUser);
        }

        await UpdateLatest(latestMessage);
        await _context.SaveChangesAsync();
        return Response.NoContent;
    }

    private async Task UpdateLatest(int? latest) {
        if (latest is null) return;

        var entity = await _context.Latests.FirstOrDefaultAsync();
        if (entity is null) {
            await _context.Latests.AddAsync(new Latest { latest = latest ?? 0 });
            return;
        }

        entity.latest = latest ?? 0;
    }
}