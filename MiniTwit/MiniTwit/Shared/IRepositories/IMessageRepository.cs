using Microsoft.AspNetCore.Mvc;
using MiniTwit.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTwit.Shared.IRepositories;
public interface IMessageRepository
{
    public Task<List<MessageDTO>> Get();
    public Task<MessageDTO> GetMessageById(int id);
    public Task<IActionResult> AddMessage();
}
