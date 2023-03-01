using Duende.IdentityServer.Validation;
using Infrastructure.Data;
using Infrastructure.Migrations;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace Infrastructure.Repositories;

public class SimRepository : ISimRepository {
    private readonly ApplicationDbContext _context;

    public SimRepository(ApplicationDbContext context) {
        _context = context;
    }

    public async Task<LatestDTO> GetAsync() {
        var entity = await _context.Latests.FirstOrDefaultAsync();

        if (entity == null) {
            return new LatestDTO { };
        }

        return new LatestDTO() { latest = entity.latest };
    }

    public async Task<Response> RegisterUser(SimUserDTO user) {
        var entity = await _context.Users.FindAsync(user.userName);

        if (entity is null) {
            entity = new ApplicationUser {
                UserName = user.userName,
                Email = user.email,
                Messages = new List<Message>(),
                Follows = new List<ApplicationUser>()
            };

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return Response.NoContent;
        }

        return Response.Conflict;

        // await _userManager.CreateAsync(new ApplicationUser 
        // {
        //     UserName = user.Name,
        //     Email = user.Email,
        // });
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
        var author = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);

        author.Messages.Add(new Message {
            Text = newMessage.content,
            PubDate = DateTime.Now,
            Author = author,
            Flagged = 0 //TODO
        });

        UpdateLatest(latestMessage);

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


    public async Task<Response> CreateFollower(string username, string Id_Target) {
        var entity = await _context.Users.FindAsync(username);

        var target = _context.Users.FirstOrDefaultAsync(u => u.Id == Id_Target);
        var targetUser = target.Result;

        if (_context.Users.FindAsync(Id_Target) == null) {
            return Response.NotFound;
        }


        // if (entity.Follows.Contains(targetUser))
        // {
        //     return Response.Conflict;
        // }
        else {
            entity.Follows.Add(targetUser);  //debugging showed error happends here
            await _context.SaveChangesAsync(); //or here
            return Response.NoContent;
        }
    }

    private async void UpdateLatest(int? latest) {
        if (latest is null) return;

        var entity = await _context.Latests.FirstOrDefaultAsync();
        if (entity is null)
        {
            await _context.Latests.AddAsync(new Latest { latest = latest ?? 0 });
            return;
        }

        entity.latest = latest ?? 0;
    }
}