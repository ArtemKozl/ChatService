using BackChat.Core.Models;

namespace BackChat.DataAccess.Repositories
{
    public interface IMessageRepository
    {
        Task Add(Messages message);
        Task DeleteGroupMessages(int groupId);
        Task<List<Messages>> GetGroupMessages(int groupId);
        Task<Messages> GetLastMessage(int groupId);
    }
}