namespace ChatSystemBackend.Application.DTO.Responses;

public class ConversationResponse
{
    public string Type { get; set; } 
    
    public string? GroupName { get; set; } 
    
    public required string AvatarUrl { get; set; }
    
    public Guid LastMessageId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}