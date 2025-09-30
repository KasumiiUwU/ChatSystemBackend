using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Application.DTO.Responses;

public class UserResponse
{
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public DateTime Created { get; set; }
    
    public string Avatar { get; set; }
    
    public Enum Role { get; set; }
    
    public OnlineStatus  OnlineStatus { get; set; }
}