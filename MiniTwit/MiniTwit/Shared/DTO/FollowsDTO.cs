namespace MiniTwit.Shared.DTO;

public record FollowsDTO {
    public string? follow { get; set; }
    public string? unfollow { get; set; }
}