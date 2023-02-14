using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;
using MiniTwit.Shared.Util;

namespace MiniTwit.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    //inject the message repository
    private readonly IMessageRepository _messageRepository;

    public MessageController(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<MessageDTO>>> Get()
    {
        return await _messageRepository.ReadAll();
    }

    [HttpGet("/message/{id}")]
    public async Task<ActionResult<MessageDTO>> GetMessageById(int id)
    {
        return (await _messageRepository.ReadAsync(id)).ToActionResult();
    }

    [HttpPost("/add_message")]
    public async Task<IActionResult> AddMessage([FromBody] MessageDTO message)
    {
        return (await _messageRepository.AddMessage(message)).ToActionResult();
    }

}
