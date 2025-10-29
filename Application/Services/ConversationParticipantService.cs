using System.Collections;
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
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ConversationParticipantService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
        _conversationParticipantRepository = unitOfWork.Repository<ConversationParticipant>("ConversationParticipants");
    }

    public async Task<IEnumerable<ConversationParticipantResponse>> CreateConversationParticipants
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
        return await _conversationParticipantRepository.ExistAsync(p =>
            p.ConversationId == conversationId && p.UserId == memberId);
    }

    public async Task<IEnumerable<Guid>> GetAllParticipantIdsFromConversation(Guid conversationId)
    {
        var participants = await _conversationParticipantRepository.GetAsync(p => p.ConversationId == conversationId);

        return participants.Select(u => u.UserId);
    }

    public async Task<IEnumerable<ConversationParticipantResponse>> GetConversationParticipantsByUserId(Guid userId)
    {
        var conversationParticipants = await 
            _conversationParticipantRepository.GetAsync(p => p.UserId == userId);
        return conversationParticipants.Select(MapToConversationResponse).ToList();
    }

    public async Task<IEnumerable<ConversationParticipantResponse>> GetConversationIdsByUserLoggingIn()
    {
        var userId = _tokenService.GetUserIdFromHttpContext(_httpContextAccessor);
        return await GetConversationParticipantsByUserId(userId);
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