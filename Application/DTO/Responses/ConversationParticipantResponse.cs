using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Application.DTO.Responses;

public class ConversationParticipantResponse
{
    [BsonRepresentation(BsonType.String)] 
    public Guid Id { get; set; }
    [BsonRepresentation(BsonType.String)] 
    public Guid UserId { get; set; }
    
    [BsonRepresentation(BsonType.String)] 
    public Guid ConversationId { get; set; }

    public required string Role { get; set; } 
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}