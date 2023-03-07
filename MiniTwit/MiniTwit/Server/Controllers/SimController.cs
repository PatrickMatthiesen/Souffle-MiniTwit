using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Server.Util;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace MiniTwit.Server.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class SimController : ControllerBase {
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISimRepository _simRepository;


    public SimController(ISimRepository simRepository, IUserRepository userRepository, IMessageRepository messageRepository) {
        _simRepository = simRepository;
        _userRepository = userRepository;
        _messageRepository = messageRepository;

    }

    [HttpGet("latest")]
    public async Task<ActionResult<LatestDTO>> GetLatest()
    // Possibly return a LatestDTO. Definitely not a MessageController, just a placeholder.
    {
        return await _simRepository.GetAsync();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] SimUserDTO user,
        [FromQuery(Name = "latest")]
         int? latestMessage) {
        return (await _simRepository.RegisterUser(user)).ToActionResult();
    }


    [HttpGet("msgs")]
    public async Task<ActionResult<List<MessageDTO>>> GetMsgs() {
        return await _messageRepository.ReadAll();
    }

    [HttpGet("msgs/{id}")]
    public async Task<ActionResult<List<MessageDTO>>> GetMsgsFromUserID(string userID) {
        return await _messageRepository.ReadByUserId(userID);
    }

    [HttpPost("msgs/{username}")]
    public async Task<IActionResult> CreateMessage(
        string username,
        [FromBody] SimMessageDTO newMessage,
        [FromQuery(Name = "latest")]
        int? latestMessage
        ) {
        var entity = new CreateMessageDTO { Text = newMessage.content };
        return (await _simRepository.CreateMessage(username, newMessage, latestMessage)).ToActionResult();
    }

    [HttpGet("fllws/{username}")]
    public async Task<ActionResult<List<UserDTO>>> GetFollows(string username) {
        return await _userRepository.ReadFollowsAsync(username);
    }


    [HttpPost("fllws/{username}")]
    public async Task<IActionResult> CreateFollower(string username, [FromBody] FollowsDTO body) {
        if (body.follow is not null) {
            return (await _simRepository.CreateOrRemoveFollower(username, body.follow, true)).ToActionResult();
        }
        else if (body.unfollow is not null)
        {
            return (await _simRepository.CreateOrRemoveFollower(username, body.unfollow, false)).ToActionResult();
        }
        else {
            return NotFound();
        }

    }
}
/* 803	115045	1584061614691	follow	    Mellie Yost	    Guadalupe Rumps */
/* 815	905006	1584079148974	unfollow	Guadalupe Rumps	Blair Reasoner */