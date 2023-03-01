using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<ActionResult<List<MessageDTO>>> Get() {
        return await _messageRepository.ReadAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MessageDTO>> GetMessageById(int id) {
        return (await _messageRepository.ReadAsync(id)).ToActionResult();
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<MessageDTO>>> GetMessagesByUserId(string userId) {
        return await _messageRepository.ReadByUserId(userId);
    }

    [HttpPost("add")]
    public async Task<ActionResult<MessageDTO>> AddMessage([FromBody] CreateMessageDTO message) {
        return await _messageRepository.AddMessage(message).ToActionResult();
    }

}
