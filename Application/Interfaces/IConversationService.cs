using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Interfaces;

public interface IConversationService
{
    public Task<ConversationResponse> CreateDirectConversation(ConversationRequest request);
    public Task<IEnumerable<ConversationResponse>> GetAllConversations();
    public string CreateGroupConversation(ConversationRequest request);
    public Task<IEnumerable<ConversationResponse>> GetConversationsFromUserId(Guid userId);
    public Task<IEnumerable<ConversationResponse>> GetConversationsFromUserLoggingIn();
    

    Task<IEnumerable<string>> GetUserIdsInConversation(string conversationId);
}