using AutoMapper;
using BackChat.Core.Models;
using BackChat.DataAccess.Entities;


namespace ChatBack.Infrastructure
{
    public class MapperMaps : Profile
    {
        public MapperMaps() 
        {
            CreateMap<ChatGroupsEntity, ChatGroups>();
            CreateMap<UserChatGroupsEntity, UserChatGroups>();
            CreateMap<MessageEntity, Messages>();
        }
    }
}
