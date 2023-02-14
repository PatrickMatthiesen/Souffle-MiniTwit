using System.Net.Mime;
namespace MiniTwit.Infrastructure.Repositories;

using System;
using System.Collections.Generic;
using global::Infrastructure.Data;
using global::Infrastructure.Models;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;



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

            entity = new ApplicationUser(user.Name, user.Email);


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

    public async Task<Response> UnFollow(string Id_own, string Id_target)
    {
        var entity = await _context.Users.FindAsync(Id_own);

        var target = entity.Follows.FirstOrDefault(f => f.Id == Id_target);

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
