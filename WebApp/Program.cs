using Infrastructure.Data;
using Infrastructure.Profile;
using Infrastructure.Service;
using Infrastructure.Service.CarService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавление служб в контейнер
builder.Services.AddHttpClient<IChatService, ChatService>();  // Этого достаточно, не нужно AddSingleton или AddScoped

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddDbContext<DataContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(InfrastructureProfile));


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
