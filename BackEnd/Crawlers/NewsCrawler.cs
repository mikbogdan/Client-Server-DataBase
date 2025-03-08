using Abot2.Crawler;
using Abot2.Poco;
using HtmlAgilityPack;
using NewsParser.Models;
using NewsParser.Model;
using NewsParser.TextServices;
using System.Text.RegularExpressions;

namespace NewsParser.Crawlers
{
    public class NewsCrawler
    {
        private readonly ArticleRepository _articleRepository;

        public NewsCrawler(ArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task StartCrawlingAsync()
        {
            try
            {
                var crawlConfig = new CrawlConfiguration
                {
                    MinCrawlDelayPerDomainMilliSeconds = 1000,
                    MaxPagesToCrawl = 1,
                    IsUriRecrawlingEnabled = false,
                    IsExternalPageCrawlingEnabled = false,
                };

                var crawler = new PoliteWebCrawler(crawlConfig);
                crawler.PageCrawlCompleted += PageCrawlCompleted;

                Uri seedUri = new Uri("https://ria.ru/world/");
                await crawler.CrawlAsync(seedUri);
            }
            
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Ошибка при сохранении статьи: {ex.Message}");
            }
        }

        private void PageCrawlCompleted(object? sender, PageCrawlCompletedArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }
            var crawledPage = e.CrawledPage;

            if (crawledPage.HttpRequestException == null && crawledPage.HttpResponseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                ExtractArticleLinks(crawledPage.Content.Text);
            }
        }

        private void ExtractArticleLinks(string htmlContent)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlContent);

            var articleLinks = new List<string>();

            for (int i = 1; i <= 20; i++)
            {
                var articleNode = htmlDoc.DocumentNode.SelectSingleNode($"//div[@data-schema-position='{i}']//a");
                if (articleNode != null)
                {
                    string href = articleNode.GetAttributeValue("href", string.Empty);
                    if (!string.IsNullOrEmpty(href) && Uri.IsWellFormedUriString(href, UriKind.Absolute))
                    {
                        articleLinks.Add(href);
                    }
                }
            }

            foreach (var link in articleLinks)
            {
                ParseArticle(link).Wait();
            }
        }

        private async Task ParseArticle(string url)
        {
            using (var client = new HttpClient())
            {
                if (_articleRepository.ArticleExists(url))
                {
                    Console.WriteLine($"Статья с URL {url} уже существует. Пропускаем.");
                    return; // Пропускаем сохранение
                }
                string htmlContent = await client.GetStringAsync(url);
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop='headline']");
                string title = titleNode?.InnerText.Trim() ?? "Нет названия";

                var dateMatch = Regex.Match(htmlContent, @"<a href=""/\d{8}/"">(\d{2}:\d{2} \d{2}\.\d{2}\.\d{4})</a>", RegexOptions.IgnoreCase);
                string publishDate = dateMatch.Success ? dateMatch.Groups[1].Value : "Нет даты";

                var articleNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@itemprop='articleBody']");
                string content = articleNode != null ? StripHtml(articleNode.InnerHtml).Trim() : "Нет текста статьи";

                var fullHtml = htmlDoc.DocumentNode.OuterHtml;

                var textAnalysisService = new TextAnalysisService();
                List<Entity> entities = textAnalysisService.ExtractEntities(content);
                string entity = string.Join(", ", entities.Select(entity => entity.Value));

                var article = new Article
                {
                    ARTICLE_TITLE = title,
                    ARTICLE_URL = url,
                    ARTICLE_TEXT = content,
                    ARTICLE_DATA = publishDate,
                    FULL_HTML = fullHtml,
                    Entities = entity
                };
                _articleRepository.SaveArticle(article);
            }
        }
        private static string StripHtml(string input)
        {
            return Regex.Replace(input, @"<.*?>", string.Empty);
        }
    }
}