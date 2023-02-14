using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared.DTO;
using MiniTwit.Shared.IRepositories;

namespace Infrastructure.Repositories;
public class MessageRepository : IMessageRepository
{
    public Task<IActionResult> AddMessage()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<List<MessageDTO>>> Get()
    {
        throw new NotImplementedException();
    }

    public Task<ActionResult<MessageDTO>> GetMessageById(int id)
    {
        throw new NotImplementedException();
    }
}
