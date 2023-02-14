using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniTwit.Shared.DTO;
public record MessageDTO
{
    public string Text { get; init; }
    public DateTime PubDate { get; init; }
    public string AuthorId { get; init; }
    public int Flagged { get; init; }
}
