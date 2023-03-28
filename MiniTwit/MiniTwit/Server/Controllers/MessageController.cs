using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using MiniTwit.Server.Util;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace MiniTwit.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase {
    //inject the message repository
    private readonly IMessageRepository _messageRepository;

    public MessageController(IMessageRepository messageRepository) {
        _messageRepository = messageRepository;
    }

    [AllowAnonymous]
    [HttpGet]
    [OutputCache(Duration = 5)] //cache the result for 10 seconds
    public async Task<ActionResult<List<MessageDTO>>> Get() {
        return await _messageRepository.ReadAll();
    }

    [HttpGet("{id}")]
    [OutputCache]
    public async Task<ActionResult<MessageDTO>> GetMessageById(int id) {
        return (await _messageRepository.ReadAsync(id)).ToActionResult();
    }

    [AllowAnonymous]
    [HttpGet("user/{userName}")]
    public async Task<ActionResult<List<MessageDTO>>> GetMessagesByUserName(string userName) {
        return await _messageRepository.ReadByUserName(userName);
    }

    [HttpPost("add")]
    public async Task<ActionResult<MessageDTO>> AddMessage([FromBody] CreateMessageDTO message) {
        return await _messageRepository.AddMessage(message).ToActionResult();
    }

}
