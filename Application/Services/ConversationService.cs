using System.Reflection;
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

    public ConversationService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
        ITokenService tokenService, IConversationParticipantService conversationParticipantService)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
        _conversationParticipantService = conversationParticipantService;
        _conversationRepository = unitOfWork.Repository<Conversation>("Conversations");
    }

    //tạo conversation, tạo 2 participant cho conversation này
    public async Task<ConversationResponse> CreateDirectConversation(ConversationRequest conversationRequest)
    {
        var conversation = MapToConversation(conversationRequest);
        
        await _conversationRepository.InsertAsync(conversation);
        
        var participant = await GenerateParticipants(conversation, conversationRequest.UserReceiveId);

        
        
        return MapToConversationResponse(conversation);
    }

    public async Task<IEnumerable<ConversationResponse>> GetAllConversations()
    {
        var conversations = await _conversationRepository.GetAllAsync();
        if (conversations == null || !conversations.Any()) 
            throw new CustomExceptions.DataNotFoundException("Not found conversation");

        return conversations.Select(con => MapToConversationResponse(con)).ToList();
    }

    private async Task<IEnumerable<ConversationParticipantResponse>> GenerateParticipants(Conversation conversation, Guid targetUserId)
    {
        var inviter = _tokenService.GetUserIdFromHttpContext(_httpContextAccessor);

        //tạo 2 participants
        var participants = 
            await _conversationParticipantService.CreateConversationParticipant([
                new ConversationParticipantRequest
            {
                UserId = inviter, 
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

    public string CreateGroupConversation(ConversationRequest request)
    {
        throw new NotImplementedException();
    }

    private Conversation MapToConversation(ConversationRequest conversationRequest)
    {
        var conversation = new Conversation
        {
            Type = EnumHelper.ToEnum<ConversationType>(conversationRequest.Type),
            GroupName = conversationRequest.GroupName,
            AvatarUrl = "DefaultAvatarUrl",
        };

        return conversation;
    }
    
    
    private ConversationResponse MapToConversationResponse(Conversation conversation)
    {
        var conversationResponse = new ConversationResponse
        {
            Type = EnumHelper.ToStringValue(conversation.Type),
            GroupName = conversation.GroupName,
            AvatarUrl = "DefaultAvatarUrl",
            CreatedAt = conversation.CreatedAt,
        };

        return conversationResponse;
    }
}