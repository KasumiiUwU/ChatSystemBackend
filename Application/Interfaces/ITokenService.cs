using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Interfaces;

public interface ITokenService
{
    string GetTokenFromHttpContext(IHttpContextAccessor httpContextAccessor);

    Task<Guid> GetUserIdFromHttpContext(IHttpContextAccessor httpContextAccessor);

    string GenerateToken(UserResponse user);
}