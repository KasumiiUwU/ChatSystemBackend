using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Domain.Entities;
using ChatSystemBackend.Domain.Enums;
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
    public async Task<string> CreateDirectConversation(ConversationRequest conversationRequest)
    {
        var conversation = MapConversationRequestToConversation(conversationRequest);

        await _conversationRepository.InsertAsync(conversation);

        return "asd";
    }

    private async Task GenerateParticipants(ConversationRequest conversationRequest, Conversation conversation)
    {
        //get 2 userId
        var inviter = _tokenService.GetUserIdFromHttpContext(_httpContextAccessor);

        //tạo 2 participants
        var participants = 
            await _conversationParticipantService.CreateConversationParticipant([
                new ConversationParticipantRequest
            {
                UserId = inviter.Result, 
                Role = nameof(ConversationParticipantRole.Member),
                ConversationId = conversation.Id, 
            },
            new ConversationParticipantRequest
            {
                UserId = conversationRequest.UserReceiveId, 
                Role = nameof(ConversationParticipantRole.Member),
                ConversationId = conversation.Id, 
            }
            ]);
        
 
    }

    public string CreateGroupConversation(ConversationRequest request)
    {
        throw new NotImplementedException();
    }


    private Conversation MapConversationRequestToConversation(ConversationRequest conversationRequest)
    {
        var conversation = new Conversation
        {
            Type = EnumHelper.ToEnum<ConversationType>(conversationRequest.Type),
            GroupName = conversationRequest.GroupName,
            AvatarUrl = "DefaultAvatarUrl",
            CreatedAt = DateTime.Now
        };

        return conversation;
    }
}