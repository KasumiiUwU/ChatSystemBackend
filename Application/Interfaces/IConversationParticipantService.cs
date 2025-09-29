using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Domain.Entities;

namespace ChatSystemBackend.Application.Interfaces;

public interface IConversationParticipantService
{
    public Task<IEnumerable<ConversationParticipantResponse>> CreateConversationParticipant(IEnumerable<ConversationParticipantRequest> request);
}