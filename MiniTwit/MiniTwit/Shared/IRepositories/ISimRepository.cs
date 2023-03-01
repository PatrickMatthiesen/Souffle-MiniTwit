using MiniTwit.Shared.DTO;

namespace MiniTwit.Shared.IRepositories;

public interface ISimRepository
{
    public Task<List<MessageDTO>> GetMsgs();
    public List<MessageDTO> GetMsgsFromUserID(string id);
    public Task<Response> CreateMessage(string username, SimMessageDTO newMessage, int? latestMessage);
    public Task<List<UserDTO>> GetFollows(string username);
    
    
    public Task<Response> RegisterUser(SimUserDTO user);

    

    public Task<LatestDTO> GetAsync();
    public Task<Response> UpdateAsync(LatestDTO dto);
    

    
    // Translated from other repositories, massive code duplication;
    Task<Response> CreateFollower(string Id_own, string Id_target);
}