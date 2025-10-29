using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace ChatSystemBackend.Application.SignalRHub;

public class ChatHub : Hub
{
    private readonly IMessageService _messageService;
    private readonly IConversationService _conversationService;
    private readonly IConversationParticipantService _conversationParticipantService;

    public ChatHub(IMessageService messageService, IConversationParticipantService conversationParticipantService, IConversationService conversationService)
    {
        _messageService = messageService;
        _conversationParticipantService = conversationParticipantService;
        _conversationService = conversationService;
    }
    
    // Khi user kết nối → join tất cả group conversation mà họ tham gia
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (userId == null)
            return;

        var conversationIds = await _conversationParticipantService.GetConversationParticipantsByUserId(Guid.Parse(userId));

        foreach (var id in conversationIds)
            await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());

        await base.OnConnectedAsync();
    }

    // Khi user mở 1 conversation cụ thể
    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
    }

    // Khi user gửi tin nhắn
    public async Task SendMessage(string conversationId, string senderId, string message)
    {
        var msg = new {
            ConversationId = conversationId,
            SenderId = senderId,
            Content = message,
            SentAt = DateTime.UtcNow
        };

        // 1️⃣ Gửi đến group (những người đang mở hội thoại)
        await Clients.Group(conversationId).SendAsync("ReceiveMessage", msg);

        // 2️⃣ Đồng thời broadcast cho toàn bộ user thuộc hội thoại đó
        // để họ update danh sách conversation (sidebar)
        var participants = await _conversationService.GetUserIdsInConversation(conversationId);
        foreach (var userId in participants)
        {
            await Clients.User(userId).SendAsync("UpdateConversationList", msg);
        }
    }

}