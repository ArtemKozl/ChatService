
namespace BackChat.Application.Services
{
    public interface IUserChatGroupsService
    {
        Task AddUserGroup(int userId, int userGroupId);
        Task DeleteUserGroup(int userId, int userGroupId);
    }
}