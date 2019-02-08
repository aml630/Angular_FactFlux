using FactFluxV3.Models;
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

        public Article CreateArticleFromVideo(Rssfeeds foundFeed, SearchResult video, bool isDuplicate = false)
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

            var r = XmlReader.Create(feed.FeedLink);

            var rssArticleList = SyndicationFeed.Load(r);

            var newArticleLogic = new ArticleLogic();


            foreach (var articleItem in rssArticleList.Items)
            {
                Article newArticle = newArticleLogic.CreateArticleFromRSSItem(feed, articleItem);

                articleList.Add(newArticle);

                var wordLogLogic = new WordLogLogic();

                wordLogLogic.LogWordsUsed(newArticle);
            }

            return articleList;
        }

        public List<Article> GetArticlesFromSearchString(string word)
        {
            List<Article> orderedArticleList;

            using (var db = new FactFluxV3Context())
            {
                var findWord = db.Words.Where(x => x.Word == word).FirstOrDefault();

                var childWords = db.ParentWords.Where(x => x.ParentWordId == findWord.WordId).Select(x => x.ChildWordId).ToList();

                var childWordStrings = db.Words.Where(x => childWords.Contains(x.WordId)).ToList();

                var fullArticleList = new List<Article>();

                foreach (var wordd in childWordStrings)
                {
                    string childBegin = wordd.Word + " ";
                    string childEnd = " " + wordd.Word;
                    string childMiddle = " " + wordd.Word + " ";

                    var childArticleList = db.Article.Where(x =>
                       (x.ArticleTitle.ToLower().StartsWith(childBegin) || x.ArticleTitle.ToLower().EndsWith(childEnd) || x.ArticleTitle.ToLower().Contains(childMiddle)) && !fullArticleList.Contains(x)
                       ).ToList();

                    fullArticleList.AddRange(childArticleList);
                }

                string beginning = word + " ";
                string end = " " + word;
                string middle = " " + word + " ";

                var articleList = db.Article.Where(x =>
               (x.ArticleTitle.ToLower().StartsWith(beginning) || x.ArticleTitle.ToLower().EndsWith(end) || x.ArticleTitle.ToLower().Contains(middle)) && !fullArticleList.Contains(x)
                ).ToList();

                fullArticleList.AddRange(articleList);

                orderedArticleList = fullArticleList.OrderByDescending(x => x.DatePublished).ToList();
            }

            return orderedArticleList;
        }
    }
}
