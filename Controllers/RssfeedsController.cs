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
using Hangfire;

namespace FactFluxV3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RssfeedsController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        private readonly FactFluxV3Context Context;

        public RssfeedsController(FactFluxV3Context context, IConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
        }

        // GET: api/Rssfeeds
        [HttpGet]
        public IEnumerable<Rssfeeds> GetRssfeeds()
        {
            return Context.Rssfeeds;
        }

        // GET: api/Rssfeeds/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRssfeeds([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var rssfeeds = await Context.Rssfeeds.FindAsync(id);

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

            Context.Entry(rssfeeds).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
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
                Context.Rssfeeds.Add(rssfeeds);
                await Context.SaveChangesAsync();
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

            var rssfeeds = await Context.Rssfeeds.FindAsync(id);
            if (rssfeeds == null)
            {
                return NotFound();
            }

            Context.Rssfeeds.Remove(rssfeeds);
            await Context.SaveChangesAsync();

            return Ok(rssfeeds);
        }

        private bool RssfeedsExists(int id)
        {
            return Context.Rssfeeds.Any(e => e.FeedId == id);
        }

        [HttpPost("{id}/GetArticles")]
        public List<Article> GetFeedArticles([FromRoute] int id)
        {
            Rssfeeds foundFeed;

            List<Article> articleList;

            using (FactFluxV3Context db = new FactFluxV3Context())
            {
                foundFeed = db.Rssfeeds.Where(x => x.FeedId == id).FirstOrDefault();
            }

            articleList = GetAllResourcesFromFeed(foundFeed);

            return articleList;
        }

        public List<Article> GetAllResourcesFromFeed(Rssfeeds foundFeed)
        {
            List<Article> articleList;

            var articleLogic = new ArticleLogic();

            articleList = articleLogic.CheckNewsEntityForArticles(foundFeed);

            var newYouTubeLogic = new YouTubeLogic(Configuration);

            var vidList = newYouTubeLogic.CheckNewsEntityForVideos(foundFeed);

            articleList.AddRange(vidList);
            return articleList;
        }


        [HttpPost("GetAllArticles")]
        public string GetArticlessForAll()
        {
            List<Rssfeeds> allFeeds;

            using (FactFluxV3Context db = new FactFluxV3Context())
            {
                allFeeds = db.Rssfeeds.ToList();

                foreach (var feed in allFeeds)
                {
                    BackgroundJob.Enqueue(() => GetAllResourcesFromFeed(feed));
                }

                var allTwitterAccounts = db.TwitterUsers.ToList();

                var twitterLogic = new TwitterLogic(Configuration);

                foreach (var twtUser in allTwitterAccounts)
                {
                    BackgroundJob.Enqueue(() => twitterLogic.GetAllResourcesFromTwitterUser(twtUser));
                }
            }

            return "Success";
        }
    }
}