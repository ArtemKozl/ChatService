using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackChat.DataAccess.Entities
{
    public class UserChatGroupsEntity
    {
        public int id { get; set; }
        public int userid { get; set; }
        public string groups { get; set; } = string.Empty;

    }
}
