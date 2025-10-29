using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using ChatSystemBackend.Domain.Enums;
using ChatSystemBackend.Exceptions;
using ChatSystemBackend.Utils;

namespace ChatSystemBackend.Application.Services;

public class ConversationService : IConversationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationRepository<Conversation> _conversationRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;
    private readonly IConversationParticipantService _conversationParticipantService;
    private readonly IUserService _userService;

    public ConversationService(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor httpContextAccessor,
        ITokenService tokenService,
        IConversationParticipantService conversationParticipantService,
        IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
        _conversationParticipantService = conversationParticipantService;
        _userService = userService;
        _conversationRepository = unitOfWork.Repository<Conversation>("Conversations");
    }

    // 🔹 Tạo direct conversation và participants
    public async Task<ConversationResponse> CreateDirectConversation(ConversationRequest conversationRequest)
    {
        var currentUserId = _tokenService.GetUserIdFromHttpContext(_httpContextAccessor);
        var conversation = MapToConversation(conversationRequest);

        await _conversationRepository.InsertAsync(conversation);
        await GenerateParticipants(conversation, conversationRequest.UserReceiveId);

        return await MapToConversationResponse(conversation, currentUserId);
    }

    public async Task<IEnumerable<ConversationResponse>> GetAllConversations()
    {
        var conversations = await _conversationRepository.GetAllAsync();
        if (!conversations.Any())
            throw new CustomExceptions.DataNotFoundException("No conversations found.");

        return conversations.Select(MapToConversationResponseBase);
    }

    // 🔹 Tạo 2 participants (người gửi & người nhận)
    private async Task<IEnumerable<ConversationParticipantResponse>> GenerateParticipants(
        Conversation conversation,
        Guid targetUserId)
    {
        var inviterId = _tokenService.GetUserIdFromHttpContext(_httpContextAccessor);

        var participants = await _conversationParticipantService.CreateConversationParticipants([
            new ConversationParticipantRequest
            {
                UserId = inviterId,
                Role = EnumHelper.ToStringValue(ConversationParticipantRole.Member),
                ConversationId = conversation.Id,
            },
            new ConversationParticipantRequest
            {
                UserId = targetUserId,
                Role = EnumHelper.ToStringValue(ConversationParticipantRole.Member),
                ConversationId = conversation.Id,
            }
        ]);

        return participants;
    }

    // 🔹 Chưa implement group
    public string CreateGroupConversation(ConversationRequest request)
    {
        throw new NotImplementedException();
    }

    // 🔹 Lấy tất cả conversation theo userId
    public async Task<IEnumerable<ConversationResponse>> GetConversationsFromUserId(Guid userId)
    {
        // Lấy danh sách participant của user
        var participants = await _conversationParticipantService.GetConversationParticipantsByUserId(userId);

        if (participants == null || !participants.Any())
            return new List<ConversationResponse>();

        var conversationResponses = new List<ConversationResponse>();

        // Duyệt qua từng participant để lấy thông tin conversation
        foreach (var participant in participants)
        {
            var conversation = await _conversationRepository.GetByIdAsync(participant.ConversationId);
            if (conversation == null)
                continue;

            var response = await MapToConversationResponse(conversation, userId);
            conversationResponses.Add(response);
        }

        return conversationResponses;
    }

    // 🔹 Lấy tất cả conversation của user đang đăng nhập
    public async Task<IEnumerable<ConversationResponse>> GetConversationsFromUserLoggingIn()
    {
        var userId = _tokenService.GetUserIdFromHttpContext(_httpContextAccessor);
        return await GetConversationsFromUserId(userId);
    }

    public Task<IEnumerable<string>> GetUserIdsInConversation(string conversationId)
    {
        throw new NotImplementedException();
    }

    // 🔹 Mapping ConversationRequest → Conversation Entity
    private static Conversation MapToConversation(ConversationRequest conversationRequest)
    {
        return new Conversation
        {
            Type = EnumHelper.ToEnum<ConversationType>(conversationRequest.Type),
            GroupName = conversationRequest.GroupName,
            AvatarUrl = "DefaultAvatarUrl"
        };
    }

    // 🔹 Hàm chính để map conversation sang response
    private async Task<ConversationResponse> MapToConversationResponse(Conversation conversation, Guid currentUserId)
    {
        return conversation.Type switch
        {
            ConversationType.Direct => await MapDirectConversation(conversation, currentUserId),
            ConversationType.Group  => MapGroupConversation(conversation),
            _                       => MapToConversationResponseBase(conversation)
        };
    }

    // 🔸 Map Direct
    private async Task<ConversationResponse> MapDirectConversation(Conversation conversation, Guid currentUserId)
    {
        var response = MapToConversationResponseBase(conversation);

        var participantIds = await _conversationParticipantService
            .GetAllParticipantIdsFromConversation(conversation.Id);

        var otherUserId = participantIds.FirstOrDefault(id => id != currentUserId);
        if (otherUserId == Guid.Empty)
            return response;

        var otherUser = await _userService.GetUserByIdAsync(otherUserId);

        response.GroupName = otherUser.Username;
        response.AvatarUrl = otherUser.Avatar;

        return response;
    }

    // 🔸 Map Group
    private static ConversationResponse MapGroupConversation(Conversation conversation)
    {
        var response = MapToConversationResponseBase(conversation);
        response.GroupName = conversation.GroupName ?? "Unnamed Group";
        response.AvatarUrl = conversation.AvatarUrl ?? "DefaultGroupAvatarUrl";
        return response;
    }

    // 🔸 Base Response chung
    private static ConversationResponse MapToConversationResponseBase(Conversation conversation)
    {
        return new ConversationResponse
        {
            Id = conversation.Id,
            Type = EnumHelper.ToStringValue(conversation.Type),
            GroupName = conversation.GroupName,
            AvatarUrl = conversation.AvatarUrl,
            CreatedAt = conversation.CreatedAt
        };
    }
}
