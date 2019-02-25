using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
using FactFluxV3.Logic;

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
        public List<TimelineArticle> GetTimelineArticle([FromRoute] string word, int[] articleTypes, int page = 1, int pageSize = 10, string letterFilter = null)
        {
            var articleLogic = new ArticleLogic();

            List<TimelineArticle> orderedList = articleLogic.GetArticlesFromSearchString(word, articleTypes, letterFilter);

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