using BackChat.Application.Services;
using BackChat.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace BackChat.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ChatGroupsController : Controller
    {
        private readonly IChatGroupsService _chatGroupsService;
        private readonly IUserChatGroupsService _userChatGroupsService;

        public ChatGroupsController(IChatGroupsService chatGroupsService, 
            IUserChatGroupsService userChatGroupsService) 
        {
            _chatGroupsService = chatGroupsService;
            _userChatGroupsService = userChatGroupsService;
        }

        [Authorize(Policy = "user")]
        [HttpPost("AddGroup")]
        public async Task<IResult> AddGroup([FromBody] GroupRequest request)
        {
            var token = Request.Cookies["tasty-cookie"];

            if (!string.IsNullOrEmpty(token))
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;

                int groupId = await _chatGroupsService.AddGroup(request.name);

                await _userChatGroupsService.AddUserGroup(Convert.ToInt32(userId), groupId);

                return Results.Ok(groupId);
            }
            else
            {
                return Results.Unauthorized();
            }
            
        }

        [Authorize (Policy = "user")]
        [HttpPost("DeleteGroup")]
        public async Task<IResult> DeleteGroup([FromBody] GroupRequest request)
        {
            var token = Request.Cookies["tasty-cookie"];

            if (!string.IsNullOrEmpty(token))
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;

                await _chatGroupsService.DeleteGroup(request.id);

                await _userChatGroupsService.DeleteUserGroup(Convert.ToInt32(userId), request.id);

            }
            else
            {
                return Results.Unauthorized();
            }
            return Results.Ok();
        }



        [Authorize(Policy = "user")]
        [HttpGet("GetGroups")]
        public async Task<IResult> GetGroups()
        {
            var token = Request.Cookies["tasty-cookie"];

            if (!string.IsNullOrEmpty(token))
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;

                var groups = await _chatGroupsService.GetAllGroups(Convert.ToInt32(userId));

                return Results.Ok(groups);
            }
            else
            {
                return Results.Unauthorized();
            }
        }
        [Authorize(Policy = "user")]
        [HttpPost("addUserToGroup")]
        public async Task AddUserToGroup([FromBody] UserToAddRequest request)
        {
            await _userChatGroupsService.AddUserGroup(request.userId, request.groupId);
        }

        [Authorize(Policy = "user")]
        [HttpPost("QuitGroup")]
        public async Task<IResult> QuitGroup([FromBody] GroupRequest request)
        {
            var token = Request.Cookies["tasty-cookie"];

            if (!string.IsNullOrEmpty(token))
            {
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "userId")?.Value;

                await _userChatGroupsService.DeleteUserGroup(Convert.ToInt32(userId), request.id);

            }
            else
            {
                return Results.Unauthorized();
            }
            return Results.Ok();
        }
        


    }
}

