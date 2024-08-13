using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackChat.Core.Models
{
    public class MessageWithByteImage
    {
        public string userName { get; set; } = string.Empty;
        public string time { get; set; } = string.Empty ;
        public string? messageText { get; set; }
        public byte[]? image { get; set; }
    }
}
