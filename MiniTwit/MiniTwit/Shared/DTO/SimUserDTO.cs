namespace MiniTwit.Shared.DTO;

public record SimUserDTO
{
    public string userName {get; set;}
    public string email {get; set;}
    public string password {get; set;}
    public int latest {get; set;}
}