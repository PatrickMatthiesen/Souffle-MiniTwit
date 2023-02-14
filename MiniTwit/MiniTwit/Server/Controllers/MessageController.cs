using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Shared.DTO;

namespace MiniTwit.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public MessageController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<MessageDTO>>> Get()
    {
        return await _context.Messages.Select(m => new MessageDTO
        {
            Text = m.Text,
            PubDate = m.PubDate,
            AuthorId = m.Id,
            Flagged = m.Flagged
        }).ToListAsync();
    }

    [HttpGet("/message/{id}")]
    public async Task<ActionResult<MessageDTO>> GetMessageById(int id)
    {
        var message = await _context.Messages.FindAsync(id);

        if (message == null)
        {
            return NotFound();
        }

        return new MessageDTO
        {
            Text = message.Text,
            PubDate = message.PubDate,
            AuthorId = message.Id,
            Flagged = message.Flagged
        };
    }

    [HttpPost("/add_message/{userId}")]
    public async Task<IActionResult> AddMessageByUserId();

}
