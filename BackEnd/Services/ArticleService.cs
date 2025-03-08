using NewsParser.Crawlers;
using NewsParser.Models;
using NewsParser.Model;
using ServiceStack;

namespace NewsParser.Services
{
    [Route("/articles", "GET")]
    public class GetArticles : IReturn<List<Article>> { }

    [Route("/crawl", "POST")]
    public class CrawlRequest : IReturnVoid { }

    [Route("/search", "POST")]
    public class SearchRequest : IReturnVoid
    {
        public required string Query { get; set; }
    }

    [Route("/search/results", "GET")]
    public class GetSearchResults : IReturn<List<Article>> { }

    public class ArticleService : Service
    {
        private readonly ArticleRepository _articleRepository;
        private static string _currentQuery = "";
        public ArticleService(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public List<Article> Get(GetArticles request)
        {
            Console.WriteLine("GET /article ������");
            return _articleRepository.GetAllArticles();
        }

        public void Post(CrawlRequest request)
        {
            Console.WriteLine("POST /crawl ������");
            var crawler = new NewsCrawler(_articleRepository);
            crawler.StartCrawlingAsync().Wait();
        }
        public void Post(SearchRequest request)
        {
            Console.WriteLine("POST /search ������");
            _currentQuery = request.Query.ToString(); // ��������� ������
        }

        // ����� ��� ��������� GET-�������
        public List<Article> Get(GetSearchResults request)
        {
            Console.WriteLine("GET /search/results ������");
            Console.WriteLine($"����������: {_currentQuery}");
            if (string.IsNullOrEmpty(_currentQuery))
            {
                throw new Exception("������ �� ��� ���������.");
            }
            return _articleRepository.SearchByEntity(_currentQuery);
        }

    }
}