using MiniTwit.Shared.DTO;

namespace MiniTwit.Shared.IRepositories;

public interface ISimRepository
{
    public Task<List<MessageDTO>> GetMsgs();
    public Task<List<MessageDTO>> GetMsgsFromUserID(string id);
    public Task<Response> CreateMessage(string username, SimMessageDTO newMessage, int? latestMessage);
    public Task<List<UserDTO>> GetFollows();
    public Task<Response> CreateFollower();
    
    // Translated from other repositories, massive code duplication;
    Task<Response> Follow(string Id_own, string Id_target);
}