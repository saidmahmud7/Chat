using System.Net;
using System.Text;
using AutoMapper;
using Domain.DTO_s;
using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Response;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Service;

public class ChatService : IChatService
{
    private const string API_KEY =
        "9Pr0iWAV0cbU05VtgFTWQ8qke2EUIx99v7dCerG4AGomCGJzSi2kJQQJ99BAACHYHv6XJ3w3AAAAACOGpcgM";

    private const string ENDPOINT =
        "https://sanja-m5xrnki3-eastus2.openai.azure.com/openai/deployments/gpt-4o/chat/completions?api-version=2024-02-15-preview";

    private readonly HttpClient _httpClient;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ChatService(HttpClient httpClient, DataContext context, IMapper mapper)
    {
        _httpClient = httpClient;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ApiResponse<string?>> GetAnswer(AI prompt)
    {
        var cars = await _context.Cars.ToListAsync();
        var carsList = string.Join(", ", cars.Select(car => $"{car.Model} ({car.Color}) - {car.Price:C}"));
        var promptForAI = $"Клиент задал вопрос: {prompt.Question} если вопрос относится к машинам возьми ответ отсюда {carsList} если нет то просто ответь по своему";
                          
        _httpClient.DefaultRequestHeaders.Add("api-key", API_KEY);
        var payload = new
        {
            messages = new[]
            {
                new { role = "user", content = promptForAI }
            },
            temperature = 0.7,
            top_p = 0.95,
            max_tokens = 800,
            stream = false
        };


        var response = await _httpClient.PostAsync(ENDPOINT,
            new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));

        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine("API Response: " + jsonResponse);

        if (response.IsSuccessStatusCode)
        {
            var responseData = JsonConvert.DeserializeObject<dynamic>(jsonResponse);
            string? answer = responseData?.choices?[0]?.message?.content?.ToString();
            return new ApiResponse<string?>(answer)
            {
                Message = answer ?? "Ответ пустой"
            };
        }

        // Логируем ошибку, если произошла ошибка на сервере
        return new ApiResponse<string?>(HttpStatusCode.InternalServerError, $"Ошибка: {response.ReasonPhrase}");
    }
}