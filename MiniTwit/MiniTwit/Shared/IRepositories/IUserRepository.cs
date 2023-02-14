namespace MiniTwit.Shared.IRepositories;

using MiniTwit.Shared.DTO;


public interface IUserRepository
{
    Task<(Response Response, string UserId)> CreateAsync(UserCreateDTO user);
    Task<UserDTO> FindAsync(string userId);
    Task<List<UserDTO>> ReadFollowsAsync(string Id);
    Task<Response> UpdateAsync(UserUpdateDTO user);
    Task<Response> DeleteAsync(string userId, bool force = false);
    Task<List<MessageDTO>> ReadMessagesFromUserIdAsync(string Id);
}
