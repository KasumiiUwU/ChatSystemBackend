using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationRepository<User> _userRepository;


    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Repository<User>("Users");
    }

    public Task Login(LoginRequest loginRequest)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsUserExists(Guid userId)
    {
        return await _userRepository.ExistAsync(userId);
    }
}