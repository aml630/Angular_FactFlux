
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Microsoft.Extensions.Configuration;
using FactFluxV3.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using FactFluxV3.Logic;

namespace FactFluxV3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : ControllerBase
    {
        private readonly IConfiguration Configuration;

        public TwitterController(IConfiguration configuration)
        {
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

        [HttpPost]
        public List<TwitterUsers> GetTweetsForUsers()
        {
            var twitterLogic = new TwitterLogic(Configuration);
            return twitterLogic.GetTweetsForAllUsers();
        }
    }
}