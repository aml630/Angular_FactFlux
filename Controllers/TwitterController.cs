using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FactFluxV3.Models;
using Microsoft.Extensions.Configuration;
using FactFluxV3.Logic;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : ControllerBase
    {
        private readonly FactFluxV3Context _context;
        private readonly IConfiguration Configuration;

        public TwitterController(FactFluxV3Context context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }


        [HttpPost("AddUser/{twitterUser}")]
        public TwitterUsers CreateTwitterUser([FromRoute] string twitterUser)
        {
            TwitterUsers newTwitterUser;

            using (var db = new FactFluxV3Context())
            {
                newTwitterUser = new TwitterUsers()
                {
                    TwitterUsername = twitterUser,
                    DateCreated = DateTime.UtcNow
                };

                db.TwitterUsers.Add(newTwitterUser);
                db.SaveChanges();
            }

            return newTwitterUser;
        }

        [HttpPost("GetAllTweets")]
        public List<TwitterUsers> GetTweetsForUsers()
        {
            var twitterLogic = new TwitterLogic(Configuration);
            return twitterLogic.GetTweetsForAllUsers();
        }

        [HttpPost("AddTweetsForUser/{twitterUser}")]
        public List<Tweets> GetTweetsForSpecificUsers([FromRoute] string twitterUser)
        {
            var twitterLogic = new TwitterLogic(Configuration);

            var tweetList = twitterLogic.GetAllTweetsForUserName(twitterUser);

            return tweetList;
        }


        // GET: api/Twitter
        [HttpGet]
        public IEnumerable<TwitterUsers> GetTwitterUsers()
        {
            return _context.TwitterUsers;
        }

        // GET: api/Twitter/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTwitterUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var twitterUsers = await _context.TwitterUsers.FindAsync(id);

            if (twitterUsers == null)
            {
                return NotFound();
            }

            return Ok(twitterUsers);
        }

        // PUT: api/Twitter/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTwitterUsers([FromRoute] int id, [FromBody] TwitterUsers twitterUsers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            if (id != twitterUsers.TwitterUserId)
            {
                return BadRequest();
            }

            var twitterLogic = new TwitterLogic(Configuration);

            var userInfo = twitterLogic.GetTwitterUserInfo(twitterUsers);

            twitterUsers.Image = userInfo.ProfileImageUrlFullSize;

            _context.Entry(twitterUsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TwitterUsersExists(id))
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

        // POST: api/Twitter
        [HttpPost]
        public async Task<IActionResult> PostTwitterUsers([FromBody] TwitterUsers twitterUsers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            twitterUsers.DateCreated = DateTime.UtcNow;

            var twitterLogic = new TwitterLogic(Configuration);

            var userInfo = twitterLogic.GetTwitterUserInfo(twitterUsers);

            twitterUsers.Image = userInfo.ProfileImageUrlFullSize;

            _context.TwitterUsers.Add(twitterUsers);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTwitterUsers", new { id = twitterUsers.TwitterUserId }, twitterUsers);
        }

        // DELETE: api/Twitter/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTwitterUsers([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var twitterUser = await _context.TwitterUsers.FindAsync(id);

            var allTweets =  _context.Tweets.Where(x => x.TwitterUserId == twitterUser.TwitterUserId).ToList();

            _context.Tweets.RemoveRange(allTweets);

            if (twitterUser == null)
            {
                return NotFound();
            }

            _context.TwitterUsers.Remove(twitterUser);
            await _context.SaveChangesAsync();

            return Ok(twitterUser);
        }

        private bool TwitterUsersExists(int id)
        {
            return _context.TwitterUsers.Any(e => e.TwitterUserId == id);
        }
    }
}