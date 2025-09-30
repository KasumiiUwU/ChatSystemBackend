using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystemBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    

    
}