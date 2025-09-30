using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using LoginRequest = ChatSystemBackend.Application.DTO.Requests.LoginRequest;
using RegisterRequest = ChatSystemBackend.Application.DTO.Requests.RegisterRequest;

namespace ChatSystemBackend.Application.Interfaces;

public interface IUserService
{
    public Task<UserResponse> GetUserByUsername(string username);
    public Task<UserResponse> CreateUser(UserRequest userRequest);
    public Task<bool> IsUsernameExists(string username);
    Task<UserResponse> ValidateUserAsync(string username, string password);

}