using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;
using MiniTwit.Shared.Util;

namespace MiniTwit.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SimController : ControllerBase
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISimRepository _simRepository;
    
    [HttpGet("latest")]
    public async Task<ActionResult<MessageController>> GetLatest()
    // Possibly return a LatestDTO. Definitely not a MessageController, just a placeholder.
    {
        throw new NotImplementedException();
    }
    
    [HttpGet("msgs")]
    public async Task<ActionResult<List<MessageDTO>>> GetMsgs()
    {
        return await _messageRepository.ReadAll();
    }

    [HttpGet("msgs/{id}")]
    public async Task<ActionResult<List<MessageDTO>>> GetMsgsFromUserID(string userID)
    {
        return await _messageRepository.ReadByUserId(userID);
    }

    [HttpPost("msgs/{username}")]
    public async Task<IActionResult> CreateMessage(
        string username, 
        [FromBody] SimMessageDTO newMessage, 
        [FromQuery(Name = "latest")] 
        int? latestMessage
        )
    {
        throw new NotImplementedException();
    }

    [HttpGet("fllws/{username}")]
    public async Task<ActionResult<List<UserDTO>>> GetFollows(string username)
    {
        return await _userRepository.ReadFollowsAsync(username);
    }

    [HttpPost("fllws/{username}")]
    public async Task<IActionResult> CreateFollower(string username, [FromBody] FollowDTO FOllowDTO)
    {
        if (userName is null)
        return await _simRepository.Follow(username, targetID);
    }
}