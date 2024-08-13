using BackChat.Core.Models;
using BackChat.DataAccess.Repositories;
using System.Linq;
using System.Text.RegularExpressions;

namespace BackChat.Application.Services
{
    public class ChatGroupsService : IChatGroupsService
    {
        private readonly IChatGroupsRepository _chatGroupsRepository;
        private readonly IUserChatGroupsRepository _userChatGroupsRepository;
        private readonly IMessageRepository _messageRepository;
        public ChatGroupsService(IChatGroupsRepository chatGroupsRepository, IMessageRepository messageRepository,
            IUserChatGroupsRepository userChatGroupsRepository)
        {
            _chatGroupsRepository = chatGroupsRepository;
            _messageRepository = messageRepository;
            _userChatGroupsRepository = userChatGroupsRepository;
        }

        public async Task<object> GetAllGroups(int userId)
        {
            List<ChatGroups> chatGroups = await _chatGroupsRepository.GetGroupsByUserId(userId); ;

            if (chatGroups == null)
            {
                throw new InvalidOperationException("chatGroups is null");
            }

            List<int> userGroups = await _userChatGroupsRepository.GetUserGroups(userId);

            var groupIds = chatGroups.Select(group => group.Id).ToList();

            var uniqueGroupIds = userGroups.Except(groupIds).ToList();

            var uniqueValues = new List<int>(uniqueGroupIds);

            foreach (var id in uniqueGroupIds)
            {
                await _userChatGroupsRepository.DeleteGroupId(userId, id);
            }

            return chatGroups;
        }
        public async Task DeleteGroup(int groupId)
        {
            await _chatGroupsRepository.DeleteByGroupId(groupId);

            await _messageRepository.DeleteGroupMessages(groupId);
        }
        public async Task<int> AddGroup(string name)
        {
            int groupId = await _chatGroupsRepository.AddGroup(name);

            return groupId;
        }

    }
}
