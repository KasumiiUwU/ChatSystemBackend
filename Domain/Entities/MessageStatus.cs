using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Domain.Entities;

public class MessageStatus : BaseEntity
{
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    
    public string MessageId { get; set; } = null!;

    public bool IsRead { get; set; } = false;
    
    public DateTime? ReadAt { get; set; }
}