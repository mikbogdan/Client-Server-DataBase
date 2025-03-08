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
            // ��������� CORS
            Plugins.Add(new CorsFeature(
                allowOriginWhitelist: new[] { "http://localhost:4200" }, // ��������� ���������� �����
                allowCredentials: true,
                allowedMethods: "GET, POST, PUT, DELETE, OPTIONS",
                allowedHeaders: "Content-Type, Authorization"
            ));

            // �������� ������� ��� �������
            var dbFactory = services.BuildServiceProvider().GetRequiredService<IDbConnectionFactory>();
            using var db = dbFactory.Open();
        }
    }
}