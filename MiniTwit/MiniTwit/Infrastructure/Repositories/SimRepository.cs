using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace Infrastructure.Repositories;

public class SimRepository : ISimRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SimRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<LatestDTO> GetAsync()
    {
        var entity = await _context.Latests.FirstOrDefaultAsync();
        
        if (entity == null)
        {
            return new LatestDTO {};
        }

        return new LatestDTO() { latest = entity.latest };
    }

    public async Task<Response> RegisterUser(SimUserDTO user)
    {

        var entity = await _context.Users.FindAsync(user.userName);
        Response response;

        if (entity is null)
        {
            entity = new ApplicationUser
            {
                UserName = user.userName,
                Email = user.email,
                Messages = new List<Message>(),
                Follows = new List<ApplicationUser>()
            };

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            response = Response.Created;
        }
            else
        {
            response = Response.Conflict;
        }
        return response;

            
        // await _userManager.CreateAsync(new ApplicationUser 
        // {
        //     UserName = user.Name,
        //     Email = user.Email,
        // });

    }

    

    

    public async Task<Response> UpdateAsync(LatestDTO dto)
    {
        var entity = await _context.Latests.FirstOrDefaultAsync();
        entity.latest = dto.latest;
        await _context.SaveChangesAsync();
        return Response.Created;       
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