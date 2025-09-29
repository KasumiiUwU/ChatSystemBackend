using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Application.DTO.Requests;

public class ConversationParticipantRequest
{
    [BsonRepresentation(BsonType.String)] 
    public Guid UserId { get; set; }
    
    [BsonRepresentation(BsonType.String)] 
    public Guid ConversationId { get; set; }

    public string Role { get; set; } 
    
}