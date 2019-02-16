using FactFluxV3.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;

namespace FactFluxV3.Logic
{
    public class TwitterLogic
    {

        private readonly IConfiguration Configuration;

        public TwitterLogic(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public List<Tweets> GetTweetsForUser(TwitterUsers twitterUser)
        {
            Auth.SetUserCredentials(Configuration["IntegrationSettings:Twitter:ConsumerKey"],
                                    Configuration["IntegrationSettings:Twitter:ConsumerSecret"],
                                    Configuration["IntegrationSettings:Twitter:AccessToken"],
                                    Configuration["IntegrationSettings:Twitter:AccessTokenSecret"]);

            var realTwitterUser =  User.GetUserFromScreenName(twitterUser.TwitterUsername);

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

        public List<TwitterUsers> GetTweetsForAllUsers()
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
    }
}
