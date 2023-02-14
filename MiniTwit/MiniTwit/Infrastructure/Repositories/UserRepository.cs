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

    public (Response Response, string UserId) Create(UserCreateDTO user)
    {
        var entity = _context.Users.FirstOrDefault ( u => u.Email == user.Email);

        Response response;
        
        if (entity is null) {

            entity = new ApplicationUser(user.Name, user.Email);
            

            _context.AddAsync(entity);
            _context.SaveChangesAsync();

            response = Response.Created;
        } else {
            response = Response.Conflict;
        }
        return (response, entity.Id);
    
    }

    
    public UserDTO Find(string userId)
    {
        var entity = _context.Users.FirstOrDefault(u => u.Id == userId);

        return new UserDTO (userId, entity.UserName, entity.Email);
        
    }

     public Response Update(UserUpdateDTO user)
    {
        var entity = _context.Users.FirstOrDefault(u => u.Id == user.Id);
        entity.Id = user.Id;
        entity.UserName = user.Name;
        entity.Email = user.Email;

        var entity2 = _context.Users.FirstOrDefault(u => u.Id == user.Id);
         
        if (entity is null)
        {
            return Response.NotFound;
        }
            else 
        {
            _context.Users.Update(entity);
            _context.SaveChangesAsync();

            return Response.Updated;
        }
    }
    

    public IReadOnlyCollection<UserDTO> ReadFollows(string Id)
    {
        var entity = _context.Users.FirstOrDefault(u => u.Id == u.Id);
        
        var returnList = new List<UserDTO>();
        
        foreach (var f in entity.Follows) 
        {
            new UserDTO(f.Id, f.UserName, f.Email);
        }

        return returnList;
    }

   

    public Response Delete(string userId, bool force = false)
    {
        var entity = _context.Users.FirstOrDefault(u => u.Id == userId);
        
        Response response;

        if (entity is null) 
        {
            response = Response.NotFound;
        } else {
            _context.Users.Remove(entity);
            _context.SaveChangesAsync();
            
            response = Response.Deleted;
        }

        return response;
    }
}
