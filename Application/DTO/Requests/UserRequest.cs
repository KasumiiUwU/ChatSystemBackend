using ChatSystemBackend.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatSystemBackend.Application.DTO.Requests;

public class UserRequest
{
    public string Username { get; set; }
    
    public string Password { get; set; }
    
}