using BackChat.Core.Models;

namespace BackChat.Application.Services
{
    public interface IMessageService
    {
        Task AddGroupMessage(int groupId, string userName, string time, string? messageText, string? image);
        Task<List<MessageWithByteImage>> GetGroupMessages(int groupId);
        Task<LastMessage> GetLastMessage(int groupId);
    }
}