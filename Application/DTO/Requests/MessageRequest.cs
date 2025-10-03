namespace ChatSystemBackend.Application.DTO.Requests;

public class MessageRequest
{
    public Guid ConversationId { get; set; }
    
    public string Content { get; set; }
}