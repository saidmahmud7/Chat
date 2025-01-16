using Infrastructure.Service;

var builder = WebApplication.CreateBuilder(args);

// Добавление служб в контейнер
builder.Services.AddHttpClient<IChatService, ChatService>();  // Этого достаточно, не нужно AddSingleton или AddScoped

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Настройка пайплайна HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
