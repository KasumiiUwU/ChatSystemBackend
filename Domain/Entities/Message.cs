using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;

namespace ChatSystemBackend.Domain.Entities;

[Table("Messages")]
public class Message : BaseEntity
{
    [BsonRepresentation(BsonType.String)]
    public Guid ConversationId { get; set; }

    [BsonRepresentation(BsonType.String)]
    public Guid SenderId { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]    
    [BsonRepresentation(BsonType.String)]           
    public MessageType MessageType { get; set; } 
    
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}