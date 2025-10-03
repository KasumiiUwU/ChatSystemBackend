using Microsoft.AspNetCore.SignalR;

namespace ChatSystemBackend.Application.SignalRHub;

public class CustomUserProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        //var claims = connection.User.Claims.Select(c => $"{c.Type}: {c.Value}").ToList();
        
        var userId = connection.User?.FindFirst("id")?.Value;

        return userId;
    }
}