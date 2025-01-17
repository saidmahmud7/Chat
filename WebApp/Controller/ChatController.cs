using Domain.Entities;
using Infrastructure.Response;
using Infrastructure.Service;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class ChatController(IChatService service) : ControllerBase
{
    [HttpPost]
    public async Task<ApiResponse<string?>> Ask([FromBody] AI prompt) => await service.GetAnswer(prompt);
    
}