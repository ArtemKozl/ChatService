
namespace BackChat.DataAccess.Repositories
{
    public interface IUserChatGroupsRepository
    {
        Task AddGroupId(int userId, int groupIdToAdd);
        Task DeleteGroupId(int userId, int groupIdToDelete);
        Task<List<int>> GetUserGroups(int userId);
    }
}