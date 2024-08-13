using AutoMapper;
using BackChat.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Text.Json;


namespace BackChat.DataAccess.Repositories
{
    public class UserChatGroupsRepository : IUserChatGroupsRepository
    {
        private readonly BackChatDbContext _context;
        private readonly IMapper _mapper;
        public UserChatGroupsRepository(BackChatDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task DeleteGroupId(int userId, int groupIdToDelete)
        {
            var row = await _context.UserChatGroups
             .AsNoTracking()
             .FirstOrDefaultAsync(u => u.userid == userId);

            if (row != null)
            {
                var options = new JsonSerializerOptions();

                if (row.groups != null)
                {
                    try
                    {
                        Dictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(row.groups, options);

                        if (result != null && result.ContainsKey("Groups"))
                        {
                            var groups = result["Groups"].ToString();

                            if (groups != null) 
                            {
                                List<int>? groupIds = JsonSerializer.Deserialize<List<int>>(groups, options);

                                if (groupIds != null)
                                {
                                    groupIds.Remove(groupIdToDelete);

                                    string updatedGroupsJson = JsonSerializer.Serialize(new { Groups = groupIds }, options);

                                    var sqlQuery = @"UPDATE ""UserChatGroups"" SET groups = to_jsonb(@groups::jsonb) WHERE userid = @userId";
                                    var parameters = new[]
                                    {
                                    new NpgsqlParameter("groups", updatedGroupsJson),
                                    new NpgsqlParameter("userId", userId)
                                };

                                    await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
                                }
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Запись не найдена");
            }

        }
        public async Task AddGroupId(int userId, int groupIdToAdd)
        {
            var row = await _context.UserChatGroups
             .AsNoTracking()
             .FirstOrDefaultAsync(u => u.userid == userId);

            if (row != null)
            {
                var options = new JsonSerializerOptions();

                if (row.groups != null)
                {
                    try
                    {
                        Dictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(row.groups, options);

                        if (result != null && result.ContainsKey("Groups"))
                        {
                            var groups = result["Groups"].ToString();

                            if(groups != null)
                            {
                                List<object>? groupIds = JsonSerializer.Deserialize<List<object>>(groups, options);

                                if (groupIds != null)
                                {
                                    groupIds.Add(groupIdToAdd);

                                    string updatedGroupsJson = JsonSerializer.Serialize(new { Groups = groupIds }, options);

                                    var sqlQuery = @"UPDATE ""UserChatGroups"" SET groups = to_jsonb(@groups::jsonb) WHERE userid = @userId";
                                    var parameters = new[]
                                    {
                                    new NpgsqlParameter("groups", updatedGroupsJson),
                                    new NpgsqlParameter("userId", userId)
                                };

                                    await _context.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
                                }
                            }
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Запись не найдена");
            }
        }

        public async Task<List<int>> GetUserGroups(int userId)
        {
            var row = await _context.UserChatGroups
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.userid == userId);

            if (row != null)
            {
                var options = new JsonSerializerOptions();
                Dictionary<string, object>? result = JsonSerializer.Deserialize<Dictionary<string, object>>(row.groups, options);

                if (result != null && result.TryGetValue("Groups", out var groupsJson))
                {
                    var groups = groupsJson.ToString();

                    if (groups != null)
                    {
                        List<int>? groupIds = JsonSerializer.Deserialize<List<int>>(groups, options);
                        return groupIds ?? new List<int>();
                    }
                }
            }

            return new List<int>();
        }
    }
}
