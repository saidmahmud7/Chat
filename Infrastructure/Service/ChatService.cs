using System.Text;
using Domain.Entities;
using Infrastructure.Response;
using Newtonsoft.Json;

namespace Infrastructure.Service;

public class ChatService : IChatService
{
    private const string API_KEY =
        "9Pr0iWAV0cbU05VtgFTWQ8qke2EUIx99v7dCerG4AGomCGJzSi2kJQQJ99BAACHYHv6XJ3w3AAAAACOGpcgM";

    private const string ENDPOINT =
        "https://sanja-m5xrnki3-eastus2.openai.azure.com/openai/deployments/gpt-4o/chat/completions?api-version=2024-02-15-preview";

    private readonly HttpClient _httpClient;

    public ChatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApiResponse<string?>> GetAnswer(Chat prompt)
    {
        _httpClient.DefaultRequestHeaders.Add("api-key", API_KEY);

        var payload = new
        {
            messages = new object[]
            {
                new
                {
                    role = "system",
                    content = new object[]
                    {
                        new
                        {
                            type = "text",
                            text = prompt.Question
                        }
                    }
                }
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
        return new ApiResponse<string?>((int)response.StatusCode, $"Ошибка: {response.ReasonPhrase}");
    }

}