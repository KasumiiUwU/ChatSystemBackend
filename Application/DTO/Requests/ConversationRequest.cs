using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Application.DTO.Requests;

public class ConversationRequest
{
    [BsonRepresentation(BsonType.String)] 
    public Guid UserReceiveId { get; set; }
    public string Type { get; set; } 
    public string? GroupName { get; set; } 
    
    public string AvatarUrl { get; set; }
}