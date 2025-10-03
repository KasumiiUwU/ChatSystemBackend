using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using ChatSystemBackend.Domain.Enums;
using ChatSystemBackend.Utils;

namespace ChatSystemBackend.Application.Services;

public class ConversationParticipantService : IConversationParticipantService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationRepository<ConversationParticipant> _conversationParticipantRepository;

    public ConversationParticipantService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _conversationParticipantRepository = unitOfWork.Repository<ConversationParticipant>("ConversationParticipants");
    }

    public async Task<IEnumerable<ConversationParticipantResponse>> CreateConversationParticipant
        (IEnumerable<ConversationParticipantRequest> request)
    {
        var responses = new List<ConversationParticipantResponse>();

        foreach (var conversationParticipantRequest in request)
        {
            var participant = MapConversationRequestToConversation(conversationParticipantRequest);

            await _conversationParticipantRepository.InsertAsync(participant);
            
            responses.Add(MapToConversationResponse(participant));
            
        }

        return responses;
    }

    public async Task<bool> IsMember(Guid conversationId, Guid memberId)
    {
        return await _conversationParticipantRepository.ExistAsync(
            p => p.ConversationId == conversationId && p.UserId == memberId);
    }

    private static ConversationParticipant MapConversationRequestToConversation(ConversationParticipantRequest request)
    {
        return new ConversationParticipant
        {
            UserId = request.UserId,
            ConversationId = request.ConversationId,
            Role = EnumHelper.ToEnum<ConversationParticipantRole>(request.Role)
        };
    }
    private static ConversationParticipantResponse MapToConversationResponse(ConversationParticipant participant)
    {
        return new ConversationParticipantResponse
        {
            Id = participant.Id,
            UserId = participant.UserId,
            ConversationId = participant.ConversationId,
            Role = EnumHelper.ToStringValue(participant.Role),
            JoinedAt = participant.JoinedAt
        };
    }
}