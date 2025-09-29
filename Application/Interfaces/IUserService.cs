using ChatSystemBackend.Application.DTO.Requests;

namespace ChatSystemBackend.Application.Interfaces;

public interface IUserService
{
    public Task Login(LoginRequest loginRequest);
    public Task<bool> IsUserExists(Guid userId);
}