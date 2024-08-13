using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChat.DataAccess.Entities
{
    public class MessageEntity
    {
        public int id { get; set; }
        public int groupId { get; set; }
        public string userName { get; set; } = string.Empty;
        public string time {  get; set; } = string.Empty ;
        public string? messageText { get; set; }
        public string? image { get; set; }
    }
}
