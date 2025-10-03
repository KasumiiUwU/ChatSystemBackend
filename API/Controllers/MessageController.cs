using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystemBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : BaseController
{
    private readonly IMessageService _messageService;

    public MessageController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMessages()
    {
        var response = await _messageService.GetAllMessages();
        return CustomResult("Success", response);
    }

    [HttpPost("SendMessage")]
    public async Task<IActionResult> SendMessage([FromBody] MessageRequest request)
    {
        var response = await _messageService.SendMessage(request);
        return CustomResult("Success", response);
    }
}