using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using ChatSystemBackend.Domain.Enums;
using ChatSystemBackend.Exceptions;
using ChatSystemBackend.Utils;
using ChatSystemBackend.Utils.Interfaces;

namespace ChatSystemBackend.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IApplicationRepository<User> _userRepository;
    private readonly IPasswordUtils  _passwordUtils;


    public UserService(IUnitOfWork unitOfWork, ITokenService tokenService, IPasswordUtils passwordUtils)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _passwordUtils = passwordUtils;
        _userRepository = unitOfWork.Repository<User>("Users");
    }


    public async Task<UserResponse> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Username == username);
        if (user == null)  throw new CustomExceptions.DataNotFoundException("Can't find user with the given username");
        
        return MapToResponse(user);

    }

    public async Task<UserResponse> CreateUser(UserRequest userRequest)
    {
        var user = MapToUser(userRequest);
        await _userRepository.InsertAsync(user);
        return MapToResponse(user);
    }

    private static User MapToUser(UserRequest request)
    {
        var user = new User
        {
            Username = request.Username,
            Password = request.Password,
            Avatar = "Default",
            Role = UserRole.User,
            OnlineStatus = OnlineStatus.Offline
        };
        
        return user;
    }

    private static UserResponse MapToResponse(User user)
    {
        var response = new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Avatar = user.Avatar,
            Role = user.Role,
            OnlineStatus = user.OnlineStatus,
            Created = user.Created
        };
        
        return response;
    }

    public async Task<bool> IsUsernameExists(string username)
    {
        return await _userRepository.ExistAsync(u => u.Username == username);
    }

    
    public async Task<UserResponse> ValidateUserAsync(string username, string password)
    {
        var user = await _userRepository.GetFirstOrDefaultAsync(x => x.Username == username);
        
        if (user == null)
            throw new CustomExceptions.InvalidDataException("Username or password is incorrect");
        
        //check 2 password giống nhau ko
        var isMatch = _passwordUtils.VerifyPassword(password, user.Password);
        
        if (!isMatch)
            throw new CustomExceptions.InvalidDataException("Username or password is incorrect");

        
        return MapToResponse(user);
    }
}