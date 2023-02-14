namespace MiniTwit.Shared.IRepositories;

using MiniTwit.Shared.DTO;


public interface IUserRepository
{
    (Response Response, string UserId) Create(UserCreateDTO user);
    UserDTO Find(string userId);
    IReadOnlyCollection<UserDTO> ReadFollows(string userId);
    Response Update(UserUpdateDTO user);
    Response Delete(string userId, bool force = false);
}
