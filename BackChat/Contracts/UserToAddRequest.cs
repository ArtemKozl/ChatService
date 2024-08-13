using System.ComponentModel.DataAnnotations;

namespace BackChat.Contracts
{
    public record UserToAddRequest(
        [Required] int userId,
        [Required] int groupId);
    
}
