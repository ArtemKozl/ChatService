using AutoMapper;
using BackChat.Core.Models;
using BackChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BackChat.DataAccess.Repositories
{
    public class ChatGroupsRepository : IChatGroupsRepository
    {
        private readonly BackChatDbContext _context;
        private readonly IMapper _mapper;
        public ChatGroupsRepository(BackChatDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<ChatGroups>> GetGroupsByUserId(int userId)
        {
            var chatGroupIds = await _context.ChatGroups
              .FromSqlRaw($"SELECT DISTINCT \"ChatGroups\".\"Id\", \"ChatGroups\".\"Name\" FROM \"UserChatGroups\" ucg," +
              $" LATERAL jsonb_array_elements(ucg.groups -> 'Groups') AS group_id, \"ChatGroups\"" +
              $" WHERE \"ChatGroups\".\"Id\" = group_id::integer and userid = @UserId",
              new NpgsqlParameter("UserId", userId))
              .ToListAsync();

            return _mapper.Map<List<ChatGroups>>(chatGroupIds);
        }
        public async Task DeleteByGroupId(int groupId)
        {
            var group = await _context.ChatGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == groupId);
            if (group != null)
                _context.ChatGroups.Remove(group);

            await _context.SaveChangesAsync();
        }
        public async Task<int> AddGroup(string name)
        {
            var chatGroupsEntity = new ChatGroupsEntity
            {
                Name = name
            };
            await _context.ChatGroups.AddAsync(chatGroupsEntity);
            await _context.SaveChangesAsync();

            int groupId = chatGroupsEntity.Id;
            return groupId;
        }
    }
}
