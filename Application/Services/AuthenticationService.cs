using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using ChatSystemBackend.Domain.Enums;
using ChatSystemBackend.Exceptions;
using ChatSystemBackend.Utils;
using ChatSystemBackend.Utils.Interfaces;

namespace ChatSystemBackend.Application.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;
    private readonly IPasswordUtils  _passwordUtils;


    public AuthenticationService(ITokenService tokenService, IUserService userService, IPasswordUtils passwordUtils)
    {
        _tokenService = tokenService;
        _userService = userService;
        _passwordUtils = passwordUtils;
    }

    public async Task<LoginResponse> Login(LoginRequest loginRequest)
    {
        //check null
        if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            throw new CustomExceptions.InvalidDataException("Username and password are required");

        //check username and password
        var user = await _userService.ValidateUserAsync(loginRequest.Username, loginRequest.Password);
        
        //generate Jwt token
        var token = _tokenService.GenerateToken(user);

        return new LoginResponse
        {
            JwtToken = token
        };
    }

    public async Task<bool> Register(RegisterRequest registerRequest)
    {
        if (registerRequest.Password != registerRequest.ConfirmPassword)
        {
            throw new CustomExceptions.InvalidDataException("Passwords do not match.");
        }

        //check if the username exists
        if (await _userService.IsUsernameExists(registerRequest.Username))
        {
            throw new CustomExceptions.InvalidDataException("Username already exists.");
        }

        //create user
        var userRequest = new UserRequest
        {
            Username = registerRequest.Username,
            Password = _passwordUtils.HashPassword(registerRequest.Password)
        };

        await _userService.CreateUser(userRequest);

        return true;
    }
}