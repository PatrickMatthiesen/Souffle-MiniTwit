using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared.DTO;

namespace MiniTwit.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpGet]
    public ActionResult<List<UserDTO>> GetFollowers (string user)
    {
        return Ok("Hello World");
    }

    
}
// username, httpget
// follow 
// unfollows