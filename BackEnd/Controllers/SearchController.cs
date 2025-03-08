//using Microsoft.AspNetCore.Mvc;
//using NewsParser.Services;

//[Route("http://localhost:5243/search")]
//[ApiController]
//public class SearchController : ControllerBase
//{
//    private readonly ArticleService _articleService;

//    public SearchController(ArticleService articleService)
//    {
//        _articleService = articleService;
//    }

//    [HttpPost]
//    public IActionResult Search([FromBody] SearchRequest request)
//    {
//        if (string.IsNullOrWhiteSpace(request.Query))
//        {
//            return BadRequest("Запрос не может быть пустым.");
//        }

//        var articles = _articleService.SearchByEntity(request.Query);
//        return Ok(articles);
//    }
//}

//public class SearchRequest
//{
//    public required string Query { get; set; }
//}