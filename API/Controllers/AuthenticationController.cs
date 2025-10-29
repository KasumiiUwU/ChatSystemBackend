using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.Interfaces;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystemBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var user = await _authenticationService.Register(registerRequest);
        return CustomResult("Đăng ký thành công! ", user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var token = await _authenticationService.Login(loginRequest);
        return CustomResult("Đăng nhập thành công", token);
    }
}