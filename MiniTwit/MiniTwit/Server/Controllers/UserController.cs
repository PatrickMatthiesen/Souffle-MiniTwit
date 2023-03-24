using Microsoft.AspNetCore.Mvc;
using MiniTwit.Server.Util;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace MiniTwit.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _repository;

    public UserController(IUserRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{userId}/follows")]
    public async Task<ActionResult<List<UserDTO>>> GetFollowers(string userId)
    {
        return await _repository.ReadFollowsAsync(userId);

    }

    [HttpGet("{userName}")]
    public async Task<List<MessageDTO>> GetAllMessages(string userName)
    {
        return await _repository.ReadMessagesFromUserNameAsync(userName);
    }

    [HttpDelete("{userId}/unfollow/{nameToUnFollow}")]
    public async Task<IActionResult> UnFollowUserByName(string userId, string nameToUnFollow)
    {
        return await _repository.UnFollowAsync(userId, nameToUnFollow).ToActionResult();
    }

    [HttpPost("{userId}/follow/{nameToFollow}")]
    public async Task<IActionResult> FollowUserByName(string userId, string nameToFollow)
    {
        return await _repository.Follow(userId, nameToFollow).ToActionResult();
    }

    [HttpGet("{userId}/mytimeline")]
    public async Task<ActionResult<List<MessageDTO>>> GetMyTimeline(string userId)
    {
        return await _repository.ReadMyTimelineAsync(userId);
    }


}
// username, httpget
