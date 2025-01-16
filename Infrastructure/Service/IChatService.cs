using Domain.Entities;
using Infrastructure.Response;

namespace Infrastructure.Service;

public interface IChatService
{
    Task<ApiResponse<string?>> GetAnswer(Chat prompt);
}