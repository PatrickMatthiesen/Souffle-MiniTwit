using MiniTwit.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Infrastructure.DbContext;
using MiniTwit.Shared;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace MiniTwit.Infrastructure.Repositories;
public class MessageRepository : IMessageRepository {
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context) {
        _context = context;
    }

    public Task<List<MessageDTO>> ReadAll()
    {
        return _context.Messages.Select(m => new MessageDTO
        {
            Text = m.Text,
            PubDate = m.PubDate,
            AuthorName = m.Author.UserName
        }).OrderByDescending(m => m.PubDate).Take(100).Reverse().ToListAsync();
    }

    /// <summary>
    /// gets a message by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Option<MessageDTO>> ReadAsync(int id) {
        var message = await _context.Messages.FindAsync(id);

        if (message is null) {
            return null; // None type
        }

        return new MessageDTO {
            Text = message.Text,
            PubDate = message.PubDate,
            AuthorName = message.Author.UserName,
        };
    }

    // get all messages from a specific user
    public Task<List<MessageDTO>> ReadByUserId(string userId) {
        return _context.Users.Include(u => u.Messages)
            .Where(u => u.Id == userId)
            .SelectMany(u => u.Messages)
            .Select(m => new MessageDTO {
                Text = m.Text,
                PubDate = m.PubDate,
                AuthorName = m.Author.UserName
            }).ToListAsync();
    }

    public async Task<Option<MessageDTO>> AddMessage(CreateMessageDTO message) {
        var author = await _context.Users.FindAsync(message.AuthorId);

        if (author is null) {
            return null;
        }

        var createdMessage = await _context.Messages.AddAsync(new Message {
            Text = message.Text,
            PubDate = DateTime.Now,
            Author = author,
            Flagged = 0 //TODO
        });

        await _context.SaveChangesAsync();
        return new MessageDTO {
            Id = createdMessage.Entity.Id,
            Text = createdMessage.Entity.Text,
            PubDate = createdMessage.Entity.PubDate,
            AuthorName = createdMessage.Entity.Author.UserName
        };
    }
}
