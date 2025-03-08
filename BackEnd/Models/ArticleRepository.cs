using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using NewsParser.Models;

namespace NewsParser.Model
{
    public class ArticleRepository
    {
        private readonly IDbConnectionFactory _dbFactory;

        public ArticleRepository(IDbConnectionFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public void SaveArticle(Article article)
        {
            using var db = _dbFactory.Open();
            db.Save(article);
        }

        public List<Article> GetAllArticles()
        {
            using var db = _dbFactory.Open();
            return db.Select<Article>();
        }
        public bool ArticleExists(string url)
        {
            using var db = _dbFactory.Open();
            return db.Exists<Article>(x => x.ARTICLE_URL == url);
        }
        public List<Article> SearchByEntity(string query)
        {
            using var db = _dbFactory.Open();
            // ѕоиск статей, где поле Entities содержит запрос
            return db.Select<Article>().Where(a => a.Entities.ToLower().Contains(query.ToLower())).ToList();
        }
    }
}