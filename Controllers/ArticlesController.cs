using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
using FactFluxV3.Logic;
using Microsoft.Extensions.Caching.Memory;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly DB_A41BC9_aml630Context Context;
        private readonly IMemoryCache Cache;

        public ArticlesController(DB_A41BC9_aml630Context context, IMemoryCache cache)
        {
            Context = context;
            Cache = cache;
        }

        // GET: api/Articles
        [HttpGet]
        public IEnumerable<Article> GetArticle()
        {
            return Context.Article;
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var article = await Context.Article.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return Ok(article);
        }

        [HttpGet("timeline/{word}")]
        public List<TimelineArticle> GetTimelineArticle([FromRoute] string word,
                                                        [FromQuery] string articleTypes,
                                                        [FromQuery] string politicalSpectrum,
                                                        [FromQuery] int page = 1,
                                                        [FromQuery] int pageSize = 20,
                                                        [FromQuery] string letterFilter = null)
        {

            var articleTypeList = new List<int>();

            if (articleTypes != null)
            {
                articleTypeList = articleTypes.Split("|").Select(Int32.Parse).ToList();
            }

            var politicalSpectrumList = new List<int>();

            if (politicalSpectrum != null)
            {
                politicalSpectrumList = politicalSpectrum.Split("|").Select(Int32.Parse).ToList();
            }

            var articleLogic = new ArticleLogic(Cache);

            List<TimelineArticle> orderedList = articleLogic.GetArticlesFromSearchString(word, page, pageSize, articleTypeList, politicalSpectrumList, letterFilter);

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

            Context.Entry(article).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
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

            Context.Article.Add(article);
            await Context.SaveChangesAsync();

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

            var article = await Context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            Context.Article.Remove(article);
            await Context.SaveChangesAsync();

            return Ok(article);
        }

        private bool ArticleExists(int id)
        {
            return Context.Article.Any(e => e.ArticleId == id);
        }
    }
}