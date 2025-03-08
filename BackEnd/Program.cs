using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.OrmLite;
using NewsParser.Model;
using NewsParser.Services;
using NewsParser;
using ServiceStack.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
             .AllowAnyHeader()                    // Разрешаем любые заголовки
              .AllowAnyMethod();
    });
});
// Регистрация зависимостей
builder.Services.AddSingleton<IDbConnectionFactory>(c =>
    new OrmLiteConnectionFactory("Server=localhost;Database=articles;User=root;Password=root;", MySqlDialect.Provider));

builder.Services.AddScoped<ArticleRepository>();
builder.Services.AddScoped<ArticleService>();

// Добавляем AppHost в контейнер зависимостей
builder.Services.AddSingleton<AppHost>();

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

// Получаем AppHost из DI-контейнера
var appHost = app.Services.GetService<AppHost>();
if (appHost == null)
{
    throw new InvalidOperationException("AppHost is not registered in the DI container.");
}

// Передаём IServiceProvider в AppHost

// Регистрируем ServiceStack middleware
app.UseServiceStack(appHost);

app.Run();