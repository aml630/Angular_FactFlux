﻿using FactFluxV3.Models;
using Google.Apis.YouTube.v3.Data;
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

            using (FactFluxV3Context db = new FactFluxV3Context())
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

            using (FactFluxV3Context db = new FactFluxV3Context())
            {
                var isDupe = db.Article.Where(x => x.ArticleTitle == newArticleLinke.ArticleTitle).FirstOrDefault();

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

            var newArticleLogic = new ArticleLogic();

            foreach (var articleItem in syndyFeed.Items)
            {
                Article newArticle = newArticleLogic.CreateArticleFromRSSItem(feed, articleItem);

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

        public List<TimelineArticle> GetArticlesFromSearchString(string word, List<int> articleTypes = null, string letterFilter = null)
        {
            List<TimelineArticle> orderedArticleList;

            using (var db = new FactFluxV3Context())
            {
                var findWord = db.Words.Where(x => x.Word == word).FirstOrDefault();

                var childWords = db.ParentWords.Where(x => x.ParentWordId == findWord.WordId).Select(x => x.ChildWordId).ToList();

                var childWordStrings = db.Words.Where(x => childWords.Contains(x.WordId)).ToList();

                var fullArticleList = new List<TimelineArticle>();

                foreach (var childWord in childWordStrings)
                {
                    List<TimelineArticle> childArticleList = GetArticlesFromWord(childWord.Word, db, fullArticleList, articleTypes);

                    fullArticleList.AddRange(childArticleList);
                }

                List<TimelineArticle> articleList = GetArticlesFromWord(word, db, fullArticleList, articleTypes);

                fullArticleList.AddRange(articleList);

                if (!string.IsNullOrEmpty(letterFilter))
                {
                    fullArticleList = articleList.Where(x => x.ArticleTitle.ToLower().Contains(letterFilter.ToLower())).ToList();
                }

                orderedArticleList = fullArticleList.OrderByDescending(x => x.DatePublished).ToList();
            }

            return orderedArticleList;
        }

        private List<TimelineArticle> GetArticlesFromWord(string word, FactFluxV3Context db, List<TimelineArticle> fullArticleList, List<int> articleTypes = null)
        {
            string beginning = word + " ";
            string end = " " + word;
            string middle = " " + word + " ";

            var articleListQuery = db.Article.Where(x =>
            (x.ArticleTitle.ToLower().StartsWith(beginning) ||
            x.ArticleTitle.ToLower().EndsWith(end) ||
            x.ArticleTitle.ToLower().Contains(middle)) &&
            !fullArticleList.Select(z => z.ArticleTitle).Contains(x.ArticleTitle));

            if (articleTypes != null)
            {
                articleListQuery = articleListQuery.Where(x => articleTypes.Contains(x.ArticleType));
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
                TimelineImage = db.Rssfeeds.Where(y => y.FeedId == x.FeedId).Select(y => y.FeedImage).FirstOrDefault()
            }).ToList();

            if (articleTypes == null || articleTypes.Contains(3))
            {
                List<TimelineArticle> tweetList = GetTweetListAsArticles(db, fullArticleList, beginning, end, middle);

                timeLineList.AddRange(tweetList);
            }

            return timeLineList;
        }

        private static List<TimelineArticle> GetTweetListAsArticles(FactFluxV3Context db, List<TimelineArticle> fullArticleList, string beginning, string end, string middle)
        {
            return db.Tweets.Where(x => (x.TweetText.ToLower().StartsWith(beginning) || x.TweetText.ToLower().EndsWith(end) || x.TweetText.ToLower().Contains(middle)) && !fullArticleList.Select(y => y.ArticleId).Contains(x.TweetId)).Select(g =>
            new TimelineArticle
            {
                ArticleId = g.TweetId,
                ArticleTitle = g.TweetText,
                ArticleUrl = g.EmbedHtml,
                ArticleType = 3,
                Active = true,
                DatePublished = g.DateTweeted,
                FeedId = g.TwitterUserId,
                TimelineImage = db.TwitterUsers.Where(y => y.TwitterUserId == g.TwitterUserId).Select(x => x.Image).FirstOrDefault()
            }).ToList();
        }
    }
}
