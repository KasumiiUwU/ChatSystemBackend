using System.Collections;
using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Interfaces;

public interface IConversationParticipantService
{
    public Task<IEnumerable<ConversationParticipantResponse>> CreateConversationParticipants(IEnumerable<ConversationParticipantRequest> request);
    public Task<bool> IsMember(Guid conversationId, Guid memberId);
    public Task<IEnumerable<Guid>> GetAllParticipantIdsFromConversation(Guid conversationId);
    Task<IEnumerable<ConversationParticipantResponse>> GetConversationParticipantsByUserId(Guid userId);
    Task<IEnumerable<ConversationParticipantResponse>> GetConversationIdsByUserLoggingIn();
    
}