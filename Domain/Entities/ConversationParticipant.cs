using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Domain.Entities;

[Table("ConversationParticipants")]
public class ConversationParticipant : BaseEntity
{
    [BsonRepresentation(BsonType.String)] 
    public Guid UserId { get; set; }
    
    [BsonRepresentation(BsonType.String)] 
    public Guid ConversationId { get; set; }

    public string Role { get; set; } 
    
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;


}