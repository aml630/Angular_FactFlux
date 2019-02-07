using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using System.ServiceModel.Syndication;
using FactFluxV3.Models;
using FactFluxV3.Logic;
using Microsoft.Extensions.Configuration;

namespace FactFluxV3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RssfeedsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly FactFluxV3Context _context;

        public RssfeedsController(FactFluxV3Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Rssfeeds
        [HttpGet]
        public IEnumerable<Rssfeeds> GetRssfeeds()
        {
            return _context.Rssfeeds;
        }

        // GET: api/Rssfeeds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRssfeeds([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rssfeeds = await _context.Rssfeeds.FindAsync(id);

            if (rssfeeds == null)
            {
                return NotFound();
            }

            return Ok(rssfeeds);
        }

        // PUT: api/Rssfeeds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRssfeeds([FromRoute] int id, [FromBody] Rssfeeds rssfeeds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != rssfeeds.FeedId)
            {
                return BadRequest();
            }

            _context.Entry(rssfeeds).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RssfeedsExists(id))
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

        // POST: api/Rssfeeds
        [HttpPost]
        public async Task<IActionResult> PostRssfeeds([FromBody] Rssfeeds rssfeeds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            rssfeeds.LastUpdated = DateTime.UtcNow;
            rssfeeds.FeedId = 0;

            try
            {
                _context.Rssfeeds.Add(rssfeeds);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return CreatedAtAction("GetRssfeeds", new { id = rssfeeds.FeedId }, rssfeeds);
        }

        // DELETE: api/Rssfeeds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRssfeeds([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rssfeeds = await _context.Rssfeeds.FindAsync(id);
            if (rssfeeds == null)
            {
                return NotFound();
            }

            _context.Rssfeeds.Remove(rssfeeds);
            await _context.SaveChangesAsync();

            return Ok(rssfeeds);
        }

        private bool RssfeedsExists(int id)
        {
            return _context.Rssfeeds.Any(e => e.FeedId == id);
        }

        [HttpPost("{id}/GetArticles")]
        public List<Article> GetFeedArticles([FromRoute] int id)
        {
            Rssfeeds foundFeed;

            var articleList = new List<Article>();

            using (FactFluxV3Context db = new FactFluxV3Context())
            {
                foundFeed = db.Rssfeeds.Where(x => x.FeedId == id).FirstOrDefault();
            }

            var r = XmlReader.Create(foundFeed.FeedLink);

            var rssArticleList = SyndicationFeed.Load(r);

            var newArticleLogic = new ArticleLogic();


            foreach (var articleItem in rssArticleList.Items)
            {
                Article newArticle = newArticleLogic.CreateArticleFromRSSFeed(foundFeed, articleItem);

                articleList.Add(newArticle);

                var rssFeedLogic = new RssfeedsLogic();

                rssFeedLogic.LogWordsUsed(newArticle);
            }

            var newYouTubeLogic = new YouTubeLogic(_configuration);

            if (foundFeed.VideoLink == null)
            {
                return articleList;
            }

            var videoList = newYouTubeLogic.GetVidsForFeed(foundFeed.VideoLink);

            var vidListResult = videoList.Where(x => x.Id.VideoId != null).ToList();

            foreach (var video in vidListResult)
            {
                var newVid = newArticleLogic.CreateArticleFromVideo(foundFeed, video);

                articleList.Add(newVid);
            }

            return articleList;
        }
    }
}