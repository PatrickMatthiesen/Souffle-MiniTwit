using Microsoft.AspNetCore.Mvc;

namespace MiniTwit.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult<List<UserDTO>> GetFollowers (string user)
    {
        return Ok("Hello World");
    }

    
}
// username, httpget
// follow 
// unfollows