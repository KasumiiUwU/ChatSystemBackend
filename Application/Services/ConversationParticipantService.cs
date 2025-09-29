using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;

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

            responses.Add(new ConversationParticipantResponse
            {
                Id = participant.Id,
                UserId = participant.UserId,
                ConversationId = participant.ConversationId,
                Role = participant.Role,
                JoinedAt = participant.JoinedAt
            });
        }

        return responses;
    }

    private static ConversationParticipant MapConversationRequestToConversation(ConversationParticipantRequest request)
    {
        return new ConversationParticipant
        {
            UserId = request.UserId,
            ConversationId = request.ConversationId,
            Role = request.Role
        };
    }
}