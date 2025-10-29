namespace ChatSystemBackend.Application.DTO.Responses;

public class ConversationResponse
{
    public Guid Id { get; set; }
    public string Type { get; set; } 
    
    public string? GroupName { get; set; } 
    
    public string AvatarUrl { get; set; }
    
    public Guid LastMessageId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}