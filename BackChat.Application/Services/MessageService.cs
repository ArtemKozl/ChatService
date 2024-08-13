using BackChat.Core.Models;
using BackChat.DataAccess.Repositories;
using ChatBack.Infrastructure;

namespace BackChat.Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IImageFileSystem _imageFileSystem;

        public MessageService(IMessageRepository messageRepository, IImageFileSystem imageFileSystem)
        {
            _messageRepository = messageRepository;
            _imageFileSystem = imageFileSystem;
        }

        public async Task AddGroupMessage(int groupId, string userName, string time, string? messageText, string? image)
        {
            var newGroupMessage = Messages.Create(groupId, userName, time, messageText, image);

            await _messageRepository.Add(newGroupMessage);
        }

        public async Task<List<MessageWithByteImage>> GetGroupMessages(int groupId)
        {
            List<Messages> messages = await _messageRepository.GetGroupMessages(groupId);

            List<MessageWithByteImage> messageWithByteImages = new List<MessageWithByteImage>();

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    MessageWithByteImage messageWithByteImage = new MessageWithByteImage()
                    {
                        userName = message.userName,
                        time = message.time,
                        messageText = message.messageText
                    };

                    if (message.image != null)
                        messageWithByteImage.image = await _imageFileSystem.ReadFileToByteArrayAsync(message.image);
                    else
                        messageWithByteImage.image = null;

                    messageWithByteImages.Add(messageWithByteImage);
                }
            }

            return messageWithByteImages;
        }

        public async Task<LastMessage> GetLastMessage(int groupId)
        {
            Messages message = await _messageRepository.GetLastMessage(groupId);
            LastMessage lastMessage = new LastMessage();

            if (message.userName == string.Empty)
            {
                lastMessage.UserName = "Chat.App";
                lastMessage.Message = "В этом чате пока нет сообщений";
                lastMessage.time = string.Empty;
            }
            else if (String.IsNullOrEmpty(message.messageText))
            {
                lastMessage.UserName = message.userName;
                lastMessage.Message = "Изображение";
                lastMessage.time = message.time;
            }
            else
            {
                lastMessage.UserName = message.userName;
                lastMessage.Message = message.messageText;
                lastMessage.time = message.time;
            }

            return lastMessage;
        }
    }
}
