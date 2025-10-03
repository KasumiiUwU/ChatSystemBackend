using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;

namespace ChatSystemBackend.Domain.Entities;

[Table("Conversations")]
public class Conversation : BaseEntity
{
    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public ConversationType Type { get; set; } 
    
    public string? GroupName { get; set; } 
    
    public required string AvatarUrl { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid LastMessageId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}