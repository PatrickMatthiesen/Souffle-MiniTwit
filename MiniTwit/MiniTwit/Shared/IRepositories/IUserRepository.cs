namespace MiniTwit.Shared.IRepositories;

using MiniTwit.Shared.DTO;

public interface IUserRepository 
{
    (Response Response, int UserId) Create(UserDTO user);
    UserDTO Find(int userId);
    IReadOnlyCollection<UserDTO> Read();
    Response Update(UserUpdateDTO user);
    Response Delete(int userId, bool force = false);
}
