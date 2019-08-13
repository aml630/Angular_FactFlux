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

        public Tweetinvi.Models.IUser GetTwitterUserInfo(TwitterUsers twitterUser)
        {
            Auth.SetUserCredentials(Configuration["IntegrationSettings:Twitter:ConsumerKey"],
                                    Configuration["IntegrationSettings:Twitter:ConsumerSecret"],
                                    Configuration["IntegrationSettings:Twitter:AccessToken"],
                                    Configuration["IntegrationSettings:Twitter:AccessTokenSecret"]);

            return User.GetUserFromScreenName(twitterUser.TwitterUsername);
        }


        public List<Tweets> GetTweetsForUser(TwitterUsers twitterUser)
        {
            Auth.SetUserCredentials(Configuration["IntegrationSettings:Twitter:ConsumerKey"],
                                    Configuration["IntegrationSettings:Twitter:ConsumerSecret"],
                                    Configuration["IntegrationSettings:Twitter:AccessToken"],
                                    Configuration["IntegrationSettings:Twitter:AccessTokenSecret"]);

            var realTwitterUser = User.GetUserFromScreenName(twitterUser.TwitterUsername);

            var recentUserTweets = Timeline.GetUserTimeline(realTwitterUser.Id, 10);

            List<Tweets> foundTweets = new List<Tweets>();

            using (var db = new DB_A41BC9_aml630Context())
            {
                var lookupUser = db.TwitterUsers.Where(x => x.TwitterUserId == twitterUser.TwitterUserId).FirstOrDefault();

                lookupUser.Image = realTwitterUser.ProfileImageUrl;

                foreach (var tweet in recentUserTweets.Where(x=>!x.IsRetweet))
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
                        if (tweet != null && tweet.Id != null)
                        {
                            var getEmbed = Tweet.GetOEmbedTweet(tweet.Id);

                            if (getEmbed != null)
                            {
                                newTweet.EmbedHtml = getEmbed.HTML;

                                db.Tweets.Add(newTweet);

                                db.SaveChanges();
                            }
                        }
                    }

                    foundTweets.Add(newTweet);
                }
            }

            return foundTweets;
        }

        public List<TwitterUsers> GetTweetsForAllUsers()
        {
            List<TwitterUsers> listOfAccounts;

            using (var db = new DB_A41BC9_aml630Context())
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

        public List<Tweets> GetAllResourcesFromTwitterUser(TwitterUsers twitterUser)
        {
            var tweetList = GetTweetsForUser(twitterUser);

            return tweetList;
        }

        public List<Tweets> GetAllTweetsForUserName(string twitterUserName)
        {
            using (var db = new DB_A41BC9_aml630Context())
            {
                var twitterUser = db.TwitterUsers.Where(x => x.TwitterUsername.ToLower() == twitterUserName.ToLower()).FirstOrDefault();

                if (twitterUser == null)
                {
                    throw new Exception("Twitter name does not exist in FactFlux");
                }

                var tweetList = GetTweetsForUser(twitterUser);

                return tweetList;
            }
        }
    }
}
