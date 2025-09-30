using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Interfaces;

public interface IConversationService
{
    public Task<Conversation> CreateDirectConversation(ConversationRequest request);
    public string CreateGroupConversation(ConversationRequest request);
}