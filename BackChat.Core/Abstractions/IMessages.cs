namespace BackChat.Core.Models
{
    public interface IMessages
    {
        int groupId { get; set; }
        int id { get; set; }
        string? image { get; set; }
        string? messageText { get; set; }
        string userName { get; set; }
    }
}