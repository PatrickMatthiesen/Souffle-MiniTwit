using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTwit.Shared.DTO;
public record MessageDTO
{
    public string Text { get; set; }
    public DateTime PubDate { get; set; }
    public string AuthorId { get; set; }
    public int Flagged { get; set; }
}

public record CreateMessageDTO
{
    public string Text { get; set; }
    public string AuthorId { get; set; }
}