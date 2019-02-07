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
                
                if(isDupe!=null)
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
            newArticleLinke.DatePublished = video.Snippet.PublishedAt?? DateTime.UtcNow;
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
    }
}
