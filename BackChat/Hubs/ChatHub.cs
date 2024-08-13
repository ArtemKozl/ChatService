using BackChat.Application.Services;
using BackChat.Models;
using ChatBack.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BackChat.Hubs
{
    public interface IChatClient
    {
        public Task ReceiveMessage(string userName, string time, string message);
        public Task ReceiveImage(string userName, string tine, byte[] image);
    }
    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IDistributedCache _cache;
        private readonly IImageFileSystem _imageFileSystem;
        private readonly IMessageService _messageService;
        public ChatHub(IDistributedCache cache, IImageFileSystem imageFileSystem, IMessageService messageService) 
        {
            _cache = cache;
            _imageFileSystem = imageFileSystem;
            _messageService = messageService;
        }

        public async Task JoinChat(UserConnection connection)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, connection.ChatRoom);

            var stringConnection = JsonSerializer.Serialize(connection);

            await _cache.SetStringAsync(Context.ConnectionId,stringConnection);

        }
        public async Task SendMessage(int groupId, string time, string message, byte[] image)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);


            if (connection != null)
            {
                if (message != null && message.Length > 0)
                {
                    await Clients
                        .Group(connection.ChatRoom)
                        .ReceiveMessage(connection.UserName, time, message);

                    await _messageService.AddGroupMessage(groupId, connection.UserName, time, message, null);
                }

                if (image != null && image.Length > 0)
                {
                    await Clients
                        .Group(connection.ChatRoom)
                        .ReceiveImage(connection.UserName, time, image);

                    string filePath = await _imageFileSystem.SaveImageAsUniqueFile(image);

                    await _messageService.AddGroupMessage(groupId, connection.UserName, time, null, filePath);
                } 
            }


            
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var stringConnection = await _cache.GetAsync(Context.ConnectionId);
            var connection = JsonSerializer.Deserialize<UserConnection>(stringConnection);

            if (connection is not null)
            {
                await _cache.RemoveAsync(Context.ConnectionId);
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, connection.ChatRoom);

                await Clients
                .Group(connection.ChatRoom)
                .ReceiveMessage("Admin", "", $"{connection.UserName} покинул чат");
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
