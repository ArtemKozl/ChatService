using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackChat.Core.Models
{
    public class UserChatGroups
    {
        public UserChatGroups(int userId, string groups)
        {
            UserId = userId;
            Groups = groups;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string Groups { get; set; } = string.Empty;
    }
}
