using BackChat.Core.Models;
using BackChat.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackChat.DataAccess.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly BackChatDbContext _context;
        private readonly ILogger<MessageRepository> _logger;
        public MessageRepository(BackChatDbContext context, ILogger<MessageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task Add(Messages message)
        {
            var messageEntity = new MessageEntity()
            {
                groupId = message.groupId,
                userName = message.userName,
                time = message.time,
                messageText = message.messageText,
                image = message.image

            };

            await _context.Messages.AddAsync(messageEntity);
            await _context.SaveChangesAsync();
        }
        public async Task<List<Messages>> GetGroupMessages(int groupId)
        {
            var messageEntities = await _context.Messages
                .AsNoTracking()
                .Where(u => u.groupId == groupId)
                .OrderBy(m => m.id)
                .ToListAsync();

            if (messageEntities.Any())
            {
                List<Messages> messagesList = messageEntities.Select(m => Messages.Create(m.groupId, m.userName, m.time, m.messageText, m.image)).ToList();

                return messagesList;
            }
            else
            {
                throw new InvalidOperationException("No messages found for the specified group ID.");
            }
        }

        public async Task<Messages> GetLastMessage(int groupId)
        {
            try
            {
                int? maxId = await _context.Messages
                    .AsNoTracking()
                    .Where(u => u.groupId == groupId)
                    .MaxAsync(r => r.id);

                var message = await _context.Messages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.id == maxId);

                if (message == null || maxId == null)
                {
                    _logger.LogInformation("No messages found for group ID {GroupId}", groupId);

                    Messages emptyMessage = Messages.Create(0, string.Empty, string.Empty, string.Empty, string.Empty);
                    return emptyMessage;
                }

                Messages lastMessage = Messages.Create(message.groupId, message.userName, message.time, message.messageText, message.image);
                return lastMessage;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error retrieving the last message for group ID {GroupId}", groupId);

                Messages emptyMessage = Messages.Create(0, string.Empty, string.Empty, string.Empty, string.Empty);
                return emptyMessage;
            }
        }

        public async Task DeleteGroupMessages(int groupId)
        {
            var messagestoRemove = await _context.Messages
                .Where(u => u.groupId == groupId)
                .ToListAsync();

            _context.Messages.RemoveRange(messagestoRemove);
            _context.SaveChanges();
        }
    }
}
