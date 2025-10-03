namespace ChatSystemBackend.Application.DTO.Responses;

public class MessageResponse
{
    public Guid Id { get; set; }

    public Guid ConversationId { get; set; }
    
    public Guid SenderId { get; set; }
          
    public string MessageType { get; set; } 
    
    public string Content { get; set; }
}