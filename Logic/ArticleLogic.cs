using FactFluxV3.Models;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace FactFluxV3.Logic
{
    public class ArticleLogic
    {
        private readonly IMemoryCache _cache;

        public ArticleLogic(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public Article CreateArticleFromRSSItem(Rssfeeds foundFeed, SyndicationItem articleItem, bool isDuplicate = false)
        {
            var newArticleLinke = new Article();

            string articleTitle = articleItem.Title.Text;
            articleTitle = articleTitle.Replace("&#8216;", "");
            articleTitle = articleTitle.Replace("&#8217;", "");

            newArticleLinke.ArticleTitle = articleTitle;
            newArticleLinke.ArticleUrl = articleItem.Links[0].Uri.AbsoluteUri;
            newArticleLinke.DatePublished = articleItem.PublishDate.UtcDateTime;
            newArticleLinke.DateAdded = DateTime.UtcNow;
            newArticleLinke.FeedId = foundFeed.FeedId;
            newArticleLinke.Active = true;
            newArticleLinke.ArticleType = 1;

            using (DB_A41BC9_aml630Context db = new DB_A41BC9_aml630Context())
            {
                var isDupe = db.Article.Where(x => x.ArticleTitle == newArticleLinke.ArticleTitle).FirstOrDefault();

                if (isDupe != null)
                {
                    isDupe.ArticleTitle = "DupeArt--" + isDupe.ArticleTitle;
                    return isDupe;
                }

                db.Article.Add(newArticleLinke);
                db.SaveChanges();
            }

            return newArticleLinke;
        }

        public Article CreateArticleFromVideo(Rssfeeds foundFeed, SearchResult video)
        {
            var newArticleLinke = new Article();

            string articleTitle = video.Snippet.Title;

            newArticleLinke.ArticleTitle = articleTitle;
            newArticleLinke.ArticleUrl = video.Id.VideoId;
            newArticleLinke.DatePublished = video.Snippet.PublishedAt ?? DateTime.UtcNow;
            newArticleLinke.DateAdded = DateTime.UtcNow;
            newArticleLinke.FeedId = foundFeed.FeedId;
            newArticleLinke.Active = true;
            newArticleLinke.ArticleType = 2;

            using (DB_A41BC9_aml630Context db = new DB_A41BC9_aml630Context())
            {
                var isDupe = db.Article.Where(x => x.ArticleTitle == newArticleLinke.ArticleTitle || x.ArticleUrl == x.ArticleUrl).FirstOrDefault();

                if (isDupe != null)
                {
                    isDupe.ArticleTitle = "DupeVid--" + isDupe.ArticleTitle;
                    return isDupe;
                }

                db.Article.Add(newArticleLinke);
                db.SaveChanges();
            }

            return newArticleLinke;
        }

        public List<Article> CheckNewsEntityForArticles(Rssfeeds feed)
        {
            var articleList = new List<Article>();

            SyndicationFeed syndyFeed;

            var r = XmlReader.Create(feed.FeedLink);

            try
            {
                syndyFeed = SyndicationFeed.Load(r);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            foreach (var articleItem in syndyFeed.Items)
            {
                Article newArticle = CreateArticleFromRSSItem(feed, articleItem);

                articleList.Add(newArticle);

                if (newArticle.ArticleTitle.StartsWith("DupeArt"))
                {
                    continue;
                }

                var wordLogLogic = new WordLogLogic();

                wordLogLogic.LogWordsUsed(newArticle);
            }

            return articleList;
        }

        public List<TimelineArticle> GetArticlesFromSearchString(string word, int page, int pageSize, List<int> articleTypes, List<int> politicalSpectrum, string letterFilter = null)
        {
            List<TimelineArticle> orderedArticleList;

            using (var db = new DB_A41BC9_aml630Context())
            {
                string spacedWord = word.Replace("-", " ");

                var findWord = db.Words.Where(x => x.Word.ToLower() == spacedWord.ToLower()).FirstOrDefault();

                var childWords = db.ParentWords.Where(x => x.ParentWordId == findWord.WordId).Select(x => x.ChildWordId).ToList();

                var childWordStrings = db.Words.Where(x => childWords.Contains(x.WordId)).ToList();

                var fullArticleList = new List<TimelineArticle>();

                foreach (var childWord in childWordStrings)
                {
                    List<TimelineArticle> childArticleList = GetArticlesFromWord(childWord.Word, db, fullArticleList, articleTypes, politicalSpectrum);

                    fullArticleList.AddRange(childArticleList);
                }

                List<TimelineArticle> articlesFromMainWord = GetArticlesFromWord(spacedWord, db, fullArticleList, articleTypes, politicalSpectrum);

                fullArticleList.AddRange(articlesFromMainWord);

                if (!string.IsNullOrEmpty(letterFilter))
                {
                    fullArticleList = fullArticleList.Where(x => x.ArticleTitle.ToLower().Contains(letterFilter.ToLower())).ToList();
                }

                orderedArticleList = fullArticleList.OrderByDescending(x => x.DatePublished).ToList();
            }

            orderedArticleList = orderedArticleList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return orderedArticleList;
        }

        private List<TimelineArticle> GetArticlesFromWord(string word, DB_A41BC9_aml630Context db, List<TimelineArticle> fullArticleList, List<int> articleTypes, List<int> politicalSpectrum)
        {
            string beginning = word + " ";
            string end = " " + word;
            string middle = " " + word + " ";

            var articleListQuery = db.Article.Where(x =>
            (x.ArticleTitle.ToLower().StartsWith(beginning) ||
            x.ArticleTitle.ToLower().EndsWith(end) ||
            x.ArticleTitle.ToLower().Contains(middle)) &&
            !fullArticleList.Select(z => z.ArticleTitle).Contains(x.ArticleTitle));

            articleListQuery = articleListQuery.Where(x => articleTypes.Contains(x.ArticleType));


            if (politicalSpectrum.FirstOrDefault() == 9)
            {
                articleListQuery = articleListQuery.Where(x => x.Feed.PoliticalSpectrum > 6);
            }

            if (politicalSpectrum.FirstOrDefault() == 1)
            {
                articleListQuery = articleListQuery.Where(x => x.Feed.PoliticalSpectrum < 4);
            }

            List<TimelineArticle> timeLineList = articleListQuery.Select(x => new TimelineArticle()
            {
                ArticleId = x.ArticleId,
                ArticleTitle = x.ArticleTitle,
                ArticleUrl = x.ArticleUrl,
                ArticleType = x.ArticleType,
                Active = true,
                DatePublished = x.DatePublished,
                FeedId = x.FeedId,
                TimelineImage = db.Rssfeeds.Where(y => y.FeedId == x.FeedId).Select(y => y.FeedImage).FirstOrDefault(),
                PoliticalSpectrum = db.Rssfeeds.Where(y => y.FeedId == x.FeedId).Select(y => y.PoliticalSpectrum).FirstOrDefault()
            }).ToList();

            if (articleTypes == null || articleTypes.Contains(3))
            {
                List<TimelineArticle> tweetList = GetTweetListAsArticles(db, fullArticleList, beginning, end, middle, politicalSpectrum);

                timeLineList.AddRange(tweetList);
            }

            return timeLineList;
        }

        private static List<TimelineArticle> GetTweetListAsArticles(DB_A41BC9_aml630Context db, List<TimelineArticle> fullArticleList, string beginning, string end, string middle, List<int> politicalSpectrum)
        {
            var tweetList = db.Tweets.Where(x => (x.TweetText.ToLower().StartsWith(beginning) || x.TweetText.ToLower().EndsWith(end) || x.TweetText.ToLower().Contains(middle)) && !fullArticleList.Select(y => y.ArticleId).Contains(x.TweetId)).Select(g =>
           new TimelineArticle
           {
               ArticleId = g.TweetId,
               ArticleTitle = g.TweetText,
               ArticleUrl = g.EmbedHtml,
               ArticleType = 3,
               Active = true,
               DatePublished = g.DateTweeted,
               FeedId = g.TwitterUserId,
               //TimelineImage = db.TwitterUsers.Where(y => y.TwitterUserId == g.TwitterUserId).Select(x => x.Image).FirstOrDefault(),
               TimelineImage = g.TwitterUser.Image,
               PoliticalSpectrum = g.TwitterUser.PoliticalSpectrum
           });

            if (politicalSpectrum.FirstOrDefault() == 9)
            {
                tweetList = tweetList.Where(x => x.PoliticalSpectrum > 6);
            }

            if (politicalSpectrum.FirstOrDefault() == 1)
            {
                tweetList = tweetList.Where(x => x.PoliticalSpectrum < 4);
            }

            return tweetList.ToList();
        }
    }
}
