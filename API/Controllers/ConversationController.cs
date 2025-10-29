using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystemBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationController  : BaseController
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _conversationService.GetAllConversations();
        return CustomResult("Success", result);
    }

    [HttpGet("getConversationsByUserLoggedIn")]
    public async Task<IActionResult> GetConversationsByUserLoggedIn()
    {
        var result = await _conversationService.GetConversationsFromUserLoggingIn();
        return CustomResult("Success", result);
    }

    [HttpPost("CreateDirectConversation")]
    public async Task<IActionResult> CreateDirectConversation([FromBody]ConversationRequest  conversationRequest)
    {
        var result = await _conversationService.CreateDirectConversation(conversationRequest);
        return CustomResult("Success", result);
    }
    
    
}