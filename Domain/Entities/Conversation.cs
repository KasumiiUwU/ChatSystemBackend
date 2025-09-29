using System.ComponentModel.DataAnnotations.Schema;
using ChatSystemBackend.Domain.Enums;

namespace ChatSystemBackend.Domain.Entities;

[Table("Conversations")]
public class Conversation : BaseEntity
{
    public ConversationType Type { get; set; } 
    
    public string? GroupName { get; set; } 
    
    public required string AvatarUrl { get; set; }
    
    public Guid LastMessageId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}