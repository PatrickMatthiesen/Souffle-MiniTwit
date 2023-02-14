using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public Task<List<MessageDTO>> ReadAll()
    {
        return _context.Messages.Select(m => new MessageDTO
        {
            Text = m.Text,
            PubDate = m.PubDate,
            AuthorId = m.Author.Id,
            Flagged = m.Flagged
        }).ToListAsync();
    }

    public async Task<Option<MessageDTO>> ReadAsync(int id)
    {
        var message = await _context.Messages.FindAsync(id);

        if (message is null)
        {
            return null; // None type
        }
        
        return new MessageDTO
        {
            Text = message.Text,
            PubDate = message.PubDate,
            AuthorId = message.Author.Id,
            Flagged = message.Flagged
        };
    }
    public async Task<Response> AddMessage(MessageDTO message)
    {
        var author = await _context.Users.FindAsync(message.AuthorId);

        if (author is null)
        {
            return Response.NotFound;
        }

        await _context.Messages.AddAsync(new Message
        {
            Text = message.Text,
            PubDate = DateTime.Now,
            Author = author,
            Flagged = message.Flagged
        });

        await _context.SaveChangesAsync();
        return Response.Created;
    }
}
