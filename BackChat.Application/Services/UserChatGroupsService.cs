using BackChat.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChat.Application.Services
{
    public class UserChatGroupsService : IUserChatGroupsService
    {
        private readonly IUserChatGroupsRepository _userChatGroupsRepository;
        public UserChatGroupsService(IUserChatGroupsRepository userChatGroupsRepository)
        {
            _userChatGroupsRepository = userChatGroupsRepository;
        }

        public async Task DeleteUserGroup(int userId, int userGroupId)
        {
            await _userChatGroupsRepository.DeleteGroupId(userId, userGroupId);
        }
        public async Task AddUserGroup(int userId, int userGroupId)
        {
            await _userChatGroupsRepository.AddGroupId(userId, userGroupId);
        }
    }
}
