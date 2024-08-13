
namespace BackChat.Application.Services
{
    public interface IChatGroupsService
    {
        Task<int> AddGroup(string name);
        Task DeleteGroup(int groupId);
        Task<object> GetAllGroups(int userId);
    }
}