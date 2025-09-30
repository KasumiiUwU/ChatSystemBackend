using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ChatSystemBackend.Application.Services;

public class TokenService : ITokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtSecurityTokenHandler  _jwtSecurityTokenHandler;
    private readonly IConfiguration _configuration;
    

    public TokenService(IHttpContextAccessor httpContextAccessor, JwtSecurityTokenHandler jwtSecurityTokenHandler, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        _configuration = configuration;
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

    public string GenerateToken(UserResponse user)
    {
        //Jwt token configuration
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];
        var expireMinutes = int.Parse(_configuration["Jwt:ExpireMinutes"] ?? "60"); // mặc định 60 phút
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //create claims
        var claim = new[]
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("username", user.Username),
            new Claim("avatar",user.Avatar)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claim,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: credentials
            );


        return _jwtSecurityTokenHandler.WriteToken(token);
    }
}