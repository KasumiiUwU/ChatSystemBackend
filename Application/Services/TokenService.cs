using System.IdentityModel.Tokens.Jwt;
using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Services;

public class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtSecurityTokenHandler  _jwtSecurityTokenHandler;
    

    public TokenService(IHttpContextAccessor httpContextAccessor, JwtSecurityTokenHandler jwtSecurityTokenHandler)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
    }


    public string GetTokenFromHttpContext(IHttpContextAccessor httpContextAccessor)
    {
        var authorizationHeader = httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
    
        if (string.IsNullOrEmpty(authorizationHeader))
        {
            throw new UnauthorizedAccessException("Token không được cung cấp.");
        }

        return authorizationHeader.Replace("Bearer ", "");
    }

    
    
    public Task<Guid> GetUserIdFromHttpContext(IHttpContextAccessor httpContextAccessor)
    {
        var token = GetTokenFromHttpContext(httpContextAccessor);
        
        return null;
    }
}