using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;

namespace ChatSystemBackend.Application.Interfaces;

public interface IAuthenticationService
{
    public Task<LoginResponse> Login(LoginRequest loginRequest);
    public Task<bool> Register(RegisterRequest registerRequest);
}