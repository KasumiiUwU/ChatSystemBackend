using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Interfaces;

public interface IConversationService
{
    public Task<string> CreateDirectConversation(ConversationRequest request);
    public string CreateGroupConversation(ConversationRequest request);
}