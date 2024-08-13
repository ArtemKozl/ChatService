using System.ComponentModel.DataAnnotations;

namespace BackChat.Contracts
{
    public record GroupRequest(
        [Required] string name,
        [Required] int id);
    
    
}
