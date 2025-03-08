using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using ServiceStack.OrmLite;
using NewsParser.Models;
using NewsParser.Services;
using ServiceStack.Data;

namespace NewsParser
{
    public class AppHost : AppHostBase
    {
        public AppHost() : base("News Parser", typeof(ArticleService).Assembly) { }

        public override void Configure(IServiceCollection services)
        {
            // Настройка CORS
            Plugins.Add(new CorsFeature(
                allowOriginWhitelist: new[] { "http://localhost:4200" }, // Разрешаем клиентскую часть
                allowCredentials: true,
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                allowedHeaders: "Content-Type, Authorization"
            ));

            // Создание таблицы при запуске
            var dbFactory = services.BuildServiceProvider().GetRequiredService<IDbConnectionFactory>();
            using var db = dbFactory.Open();
        }
    }
}