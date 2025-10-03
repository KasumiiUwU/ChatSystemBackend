using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;

namespace ChatSystemBackend.Domain.Entities;

[Table("Users")]
public class User : BaseEntity
{
    public required string Username { get; set; }
    
    public required string Password { get; set; }
    
    public DateTime Created { get; set; }
    
    public required string Avatar { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]    // JSON.Net
    [BsonRepresentation(BsonType.String)]           //Mongo
    public required UserRole Role { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]    // JSON.Net
    [BsonRepresentation(BsonType.String)]           //Mongo
    public OnlineStatus  OnlineStatus { get; set; }
    
    
}