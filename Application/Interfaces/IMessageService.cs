using ChatSystemBackend.Application.DTO.Requests;
using ChatSystemBackend.Application.DTO.Responses;

namespace ChatSystemBackend.Application.Interfaces;

public interface IMessageService
{
    public Task<IEnumerable<MessageResponse>> GetAllMessages();
    public Task<MessageResponse> SendMessage(MessageRequest message);
}