using ServiceStack.DataAnnotations;

namespace NewsParser.Models
{
    [Alias("info")]	
    public class Article
	{
        [AutoIncrement]
        public int ID { get; set; }

		public required string ARTICLE_TITLE { get; set; }
        public required string ARTICLE_URL { get; set; }
        public required string ARTICLE_TEXT { get; set; }
        public required string ARTICLE_DATA { get; set; }
		public required string FULL_HTML { get; set; }
        public required string Entities { get; set; }
    }
}