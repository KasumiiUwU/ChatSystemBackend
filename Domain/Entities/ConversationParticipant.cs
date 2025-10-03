using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;

namespace ChatSystemBackend.Domain.Entities;

[Table("ConversationParticipants")]
public class ConversationParticipant : BaseEntity
{
    [BsonRepresentation(BsonType.String)] 
    public Guid UserId { get; set; }
    
    [BsonRepresentation(BsonType.String)] 
    public Guid ConversationId { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public ConversationParticipantRole Role { get; set; } 
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;


}