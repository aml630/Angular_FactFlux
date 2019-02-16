
using Microsoft.AspNetCore.Mvc;
using Tweetinvi;
using Microsoft.Extensions.Configuration;
using FactFluxV3.Models;
using System;
using System.Linq;
using System.Collections.Generic;

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
            List<TwitterUsers> listOfAccounts;

            using (var db = new FactFluxV3Context())
            {
                listOfAccounts = db.TwitterUsers.ToList();
            }

            foreach (var acct in listOfAccounts)
            {
                var tweetList = GetTweetsForUser(acct);

                acct.Tweets = tweetList;
            }

            return listOfAccounts;
        }

        public List<Tweets> GetTweetsForUser(TwitterUsers twitterUser)
        {
            Auth.SetUserCredentials(Configuration["IntegrationSettings:Twitter:ConsumerKey"],
                                    Configuration["IntegrationSettings:Twitter:ConsumerSecret"],
                                    Configuration["IntegrationSettings:Twitter:AccessToken"],
                                    Configuration["IntegrationSettings:Twitter:AccessTokenSecret"]);

            var realTwitterUser = Tweetinvi.User.GetUserFromScreenName(twitterUser.TwitterUsername);

            var recentUserTweets = Timeline.GetUserTimeline(realTwitterUser.Id, 10);

            List<Tweets> foundTweets = new List<Tweets>();

            using (var db = new FactFluxV3Context())
            {
                var lookupUser = db.TwitterUsers.Where(x => x.TwitterUserId == twitterUser.TwitterUserId).FirstOrDefault();

                lookupUser.Image = realTwitterUser.ProfileImageUrl;

                foreach (var tweet in recentUserTweets)
                {
                    var newTweet = new Tweets()
                    {
                        EmbedHtml = "",
                        TweetText = tweet.FullText,
                        DateCreated = DateTime.UtcNow,
                        DateTweeted = tweet.TweetLocalCreationDate,
                        TwitterUserId = twitterUser.TwitterUserId
                    };

                    var isDupe = db.Tweets.Where(x => x.TweetText == tweet.FullText).FirstOrDefault();

                    if (isDupe != null)
                    {
                        newTweet.TweetText = "DupeTweet--" + isDupe.TweetText;
                    }
                    else
                    {
                        newTweet.EmbedHtml = Tweet.GetOEmbedTweet(tweet.Id).HTML;

                        db.Tweets.Add(newTweet);

                        db.SaveChanges();
                    }

                    foundTweets.Add(newTweet);
                }
            }

            return foundTweets;
        }
    }
}