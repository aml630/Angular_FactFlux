using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly FactFluxV3Context _context;

        public ArticlesController(FactFluxV3Context context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public IEnumerable<Article> GetArticle()
        {
            return _context.Article;
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Article.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpGet("timeline/{word}")]
        public List<Article> GetTimelineArticle([FromRoute] string word, int page = 1, int pageSize = 10)
        {
            var findWord = _context.Words.Where(x => x.Word == word).FirstOrDefault();

            var childWords = _context.ParentWords.Where(x => x.ParentWordId == findWord.WordId).Select(x => x.ChildWordId).ToList();

            var childWordStrings = _context.Words.Where(x => childWords.Contains(x.WordId)).ToList();

            var fullArticleList = new List<Article>();

            foreach (var wordd in childWordStrings)
            {
                string childBegin = wordd.Word + " ";
                string childEnd = " " + wordd.Word;
                string childMiddle = " " + wordd.Word + " ";

                var childArticleList = _context.Article.Where(x =>
                   (x.ArticleTitle.ToLower().StartsWith(childBegin) || x.ArticleTitle.ToLower().EndsWith(childEnd) || x.ArticleTitle.ToLower().Contains(childMiddle)) && !fullArticleList.Contains(x)
                   ).ToList();


                fullArticleList.AddRange(childArticleList);
            }

            string beginning = word + " ";
            string end = " " + word;
            string middle = " " + word + " ";

            var articleList = _context.Article.Where(x =>
           (x.ArticleTitle.ToLower().StartsWith(beginning) || x.ArticleTitle.ToLower().EndsWith(end) || x.ArticleTitle.ToLower().Contains(middle)) && !fullArticleList.Contains(x)
            ).ToList();

            fullArticleList.AddRange(articleList);

            var orderedList = fullArticleList.OrderByDescending(x => x.DatePublished).ToList();

            return orderedList;
        }


        // PUT: api/Articles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle([FromRoute] int id, [FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != article.ArticleId)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        [HttpPost]
        public async Task<IActionResult> PostArticle([FromBody] Article article)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.ArticleId }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            return Ok(article);
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ArticleId == id);
        }
    }
}