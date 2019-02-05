using FactFluxV3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace FactFluxV3.Logic
{
    public class ArticleLogic
    {
        public Article CreateArticleFromRSSFeed(Rssfeeds foundFeed, SyndicationItem articleItem, bool isDuplicate = false)
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
                    isDupe.ArticleTitle = "Dupe--" + isDupe.ArticleTitle;
                    return isDupe;
                }

                db.Article.Add(newArticleLinke);
                db.SaveChanges();
            }

            return newArticleLinke;
        }
    }
}
