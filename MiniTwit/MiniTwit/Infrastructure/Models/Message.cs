using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models;
internal class Message
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime PubDate { get; set; }
    public ApplicationUser Author { get; set; }
    public int Flagged { get; set; }
}
