using System.ComponentModel.DataAnnotations.Schema;
using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Domain.Entities;

[Table("Messages")]
public class Message : BaseEntity
{
    [BsonRepresentation(BsonType.String)]
    public Guid ConversationId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }

    public MessageType MessageType { get; set; } 
    
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}