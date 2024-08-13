using BackChat.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BackChat.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MessagesController : Controller
    {
        private readonly IMessageService _messageService;
        public MessagesController(IMessageService messageService) 
        {
            _messageService = messageService;
        }

        [Authorize(Policy = "user")]
        [HttpGet("GetMessages/{groupId}")]
        public async Task<IResult> GetMessages(int groupId)
        {
            var messages = await _messageService.GetGroupMessages(groupId);

            return Results.Ok(messages);
        }
        [Authorize(Policy = "user")]
        [HttpGet("GetLastMessage/{groupId}")]
        public async Task<IResult> GetLastMessage(int groupId)
        {
            var lastMessage = await _messageService.GetLastMessage(groupId);

            return Results.Ok(lastMessage);
        }

    }
}
