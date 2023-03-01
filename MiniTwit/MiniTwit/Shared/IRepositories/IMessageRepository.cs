using MiniTwit.Shared.DTO;

namespace MiniTwit.Shared.IRepositories;
public interface IMessageRepository {
    public Task<List<MessageDTO>> ReadAll();
    public Task<Option<MessageDTO>> ReadAsync(int id);
    public Task<List<MessageDTO>> ReadByUserId(string userId);
    public Task<Option<MessageDTO>> AddMessage(CreateMessageDTO message);
}
