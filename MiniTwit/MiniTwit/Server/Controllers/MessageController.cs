using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MiniTwit.Server.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    [HttpGet]
    public Task<ActionResult<MessageDTO>> Get()
    {
        return Ok("Hello World");
    }
}
