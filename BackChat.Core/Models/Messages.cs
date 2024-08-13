

namespace BackChat.Core.Models
{
    public class Messages : IMessages
    {
        public Messages(int groupid, string username, string timeSended, string? message, string? imageInput)
        {
            groupId = groupid;
            userName = username;
            time = timeSended;
            messageText = message;
            image = imageInput;
        }

        public int id { get; set; }
        public int groupId { get; set; }
        public string userName { get; set; } = string.Empty;
        public string time {  get; set; } = string.Empty ;
        public string? messageText { get; set; }
        public string? image { get; set; }

        public static Messages Create(int groupid, string username, string time, string? message, string? image)
        {
            var user = new Messages(groupid, username, time, message, image);
            return user;
        }
    }
}
