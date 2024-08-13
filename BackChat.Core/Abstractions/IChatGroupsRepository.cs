using BackChat.Core.Models;

namespace BackChat.DataAccess.Repositories
{
    public interface IChatGroupsRepository
    {
        Task<int> AddGroup(string name);
        Task DeleteByGroupId(int groupId);
        Task<List<ChatGroups>> GetGroupsByUserId(int userId);
    }
}