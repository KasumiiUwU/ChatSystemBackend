using System.ComponentModel.DataAnnotations.Schema;
using ChatSystemBackend.Domain.Enums;

namespace ChatSystemBackend.Domain.Entities;

[Table("Users")]
public class User : BaseEntity
{
    public required string Username { get; set; }
    
    public required string Password { get; set; }
    
    public DateTime Created { get; set; }
    
    public required string Avatar { get; set; }
    
    public OnlineStatus  OnlineStatus { get; set; }
    
    
}