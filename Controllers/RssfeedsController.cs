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
using Microsoft.Extensions.Caching.Memory;

namespace FactFluxV3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RssfeedsController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        private readonly DB_A41BC9_aml630Context Context;

        private readonly IMemoryCache Cache;

        public RssfeedsController(DB_A41BC9_aml630Context context, IConfiguration configuration, IMemoryCache cache)
        {
            Context = context;
            Configuration = configuration;
            Cache = cache;
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

            rssfeeds.LastUpdated = DateTime.UtcNow;

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

            var articlesTORemove = Context.Article.Where(x => x.FeedId == id);

            Context.Article.RemoveRange(articlesTORemove);

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

            using (DB_A41BC9_aml630Context db = new DB_A41BC9_aml630Context())
            {
                foundFeed = db.Rssfeeds.Where(x => x.FeedId == id).FirstOrDefault();
            }

            articleList = GetAllResourcesFromFeed(foundFeed);

            return articleList;
        }

        public List<Article> GetAllResourcesFromFeed(Rssfeeds foundFeed)
        {
            List<Article> articleList = new List<Article>();

            if (!string.IsNullOrEmpty(foundFeed.FeedLink))
            {
                var articleLogic = new ArticleLogic(Cache);

                var feedArticles = articleLogic.CheckNewsEntityForArticles(foundFeed);

                articleList.AddRange(feedArticles);
            }

            if (!string.IsNullOrEmpty(foundFeed.VideoLink))
            {
                var newYouTubeLogic = new YouTubeLogic(Configuration, Cache);

                var vidList = newYouTubeLogic.CheckNewsEntityForVideos(foundFeed);

                articleList.AddRange(vidList);
            }

            return articleList;
        }

        [HttpPost("CreateDailyCheck")]
        public string CreateDailyCheck()
        {
            List<Rssfeeds> allFeeds;

            using (DB_A41BC9_aml630Context db = new DB_A41BC9_aml630Context())
            {
                allFeeds = db.Rssfeeds.ToList();

                foreach (var feed in allFeeds)
                {
                    RecurringJob.AddOrUpdate("RSS:-" + feed.FeedTitle, () => GetAllResourcesFromFeed(feed), Cron.Daily);
                }

                var allTwitterAccounts = db.TwitterUsers.ToList();

                var twitterLogic = new TwitterLogic(Configuration);

                foreach (var twtUser in allTwitterAccounts)
                {
                    RecurringJob.AddOrUpdate("Twtr:-" + twtUser.TwitterUsername, () => twitterLogic.GetAllResourcesFromTwitterUser(twtUser), Cron.Daily);
                }
            }

            return "Success";
        }

        [HttpPost("GetAllArticles")]
        public string GetArticlessForAll()
        {
            List<Rssfeeds> allFeeds;

            using (DB_A41BC9_aml630Context db = new DB_A41BC9_aml630Context())
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