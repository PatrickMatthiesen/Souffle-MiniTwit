namespace MiniTwit.Shared.IRepositories;

using MiniTwit.Shared.DTO;


public interface IUserRepository {
    Task<(Response Response, string userId)> CreateAsync(UserCreateDTO user);
    Task<(Response Response, UserDTO)> FindAsync(string userId);
    Task<Response> UpdateAsync(UserUpdateDTO user);
    Task<List<UserDTO>> ReadFollowsAsync(string Id);
    Task<Response> UnFollow(string Id_own, string Id_target);
    Task<Response> Follow(string Id_own, string Id_target);
    Task<List<MessageDTO>> ReadMessagesFromUserIdAsync(string Id);
    Task<Response> DeleteAsync(string userId, bool force = false);

}
