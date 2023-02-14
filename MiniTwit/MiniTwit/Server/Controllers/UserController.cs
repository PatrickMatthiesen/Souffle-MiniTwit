using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("/{userId}/follows")]
    public async Task<ActionResult<List<UserDTO>>> GetFollowers(string userId)
    {
        return await _repository.ReadFollowsAsync(userId);

    }

    [HttpGet("/{userId}/messages")]
    public async Task<List<MessageDTO>> GetAllMessages(string userId)
    {
        return await _repository.ReadMessagesFromUserIdAsync(userId);
    }



}
// username, httpget
// follow 
// unfollows