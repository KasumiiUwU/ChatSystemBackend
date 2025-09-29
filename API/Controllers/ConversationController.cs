using ChatSystemBackend.Domain.Entities;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;

namespace ChatSystemBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationController  : BaseController
{
    [HttpGet]
    public Task<IEnumerable<Conversation>> GetAll()
    {

        return null;
    }
    
    
}