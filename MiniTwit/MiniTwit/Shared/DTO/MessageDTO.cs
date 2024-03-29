﻿namespace MiniTwit.Shared.DTO;
public record MessageDTO {
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime PubDate { get; set; }
    public string AuthorName { get; set; }
}

public record CreateMessageDTO {
    public string Text { get; set; }
    public string AuthorId { get; set; }
}