using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Domain.Entities;

public abstract class BaseEntity
{
    [Key]
    [BsonId] // MongoDB
    [BsonRepresentation(BsonType.String)] // Dùng Guid dạng string, tránh lỗi khi serialize
    public Guid Id { get; set; } = Guid.NewGuid();
}