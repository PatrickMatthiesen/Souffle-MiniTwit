namespace MiniTwit.Infrastructure.Models;
public class Message {
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime PubDate { get; set; }
    public ApplicationUser Author { get; set; }
    public int Flagged { get; set; }
}
