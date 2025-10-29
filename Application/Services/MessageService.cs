using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;
using ChatSystemBackend.Application.Interfaces;
using ChatSystemBackend.Application.SignalRHub;
using ChatSystemBackend.Domain.Entities;
using ChatSystemBackend.Domain.Enums;
using ChatSystemBackend.Exceptions;
using ChatSystemBackend.Utils;
using Microsoft.AspNetCore.SignalR;

namespace ChatSystemBackend.Application.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IApplicationRepository<Message> _messageRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ITokenService _tokenService;
    private readonly IConversationParticipantService _conversationParticipantService;
    private readonly IHubContext<ChatHub> _chatHub;

    public MessageService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, ITokenService tokenService, IConversationParticipantService conversationParticipantService, IHubContext<ChatHub> chatHub)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
        _tokenService = tokenService;
        _conversationParticipantService = conversationParticipantService;
        _chatHub = chatHub;
        _messageRepository = unitOfWork.Repository<Message>("Messages");
    }

    public async Task<IEnumerable<MessageResponse>> GetAllMessages()
    {
        var message = await _messageRepository.GetAllAsync();
        return message.Select(MapToMessageResponse).ToList();
    }

    public Task<IEnumerable<MessageResponse>> GetMessages(Guid conversationId, int pageIndex, int pageSize)
    {
        
        throw new NotImplementedException();
    }

    public async Task<MessageResponse> SendMessage(MessageRequest request)
    {
        var userIdLoggedIn = _tokenService.GetUserIdFromHttpContext(_httpContextAccessor);
        
        if (!await _conversationParticipantService.IsMember(request.ConversationId, userIdLoggedIn))
        {
            throw new CustomExceptions.InvalidDataException("Not part of the conversation");
        }
        
        var message = MapToMessage(request);
        
        //add missing field
        message.MessageType = MessageType.Text;
        message.SenderId = userIdLoggedIn;
      
        await _messageRepository.InsertAsync(message);

        var response = MapToMessageResponse(message);
        return response;
    }

    private static MessageResponse MapToMessageResponse(Message message)
    {
        return new MessageResponse
        {
            Id = message.Id,
            ConversationId = message.ConversationId,
            SenderId = message.SenderId,
            MessageType = EnumHelper.ToStringValue(message.MessageType),
            Content = message.Content
            
        };
    }

    private static Message MapToMessage(MessageRequest request)
    {
        return new Message
        {
            ConversationId = request.ConversationId,
            Content = request.Content,
        };
    }
}
