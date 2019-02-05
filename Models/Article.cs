using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Article
    {
        public Article()
        {
            WordLogs = new HashSet<WordLogs>();
        }

        public int ArticleId { get; set; }
        public string ArticleTitle { get; set; }
        public string ArticleUrl { get; set; }
        public int ArticleType { get; set; }
        public int FeedId { get; set; }
        public bool Active { get; set; }
        public DateTime DatePublished { get; set; }
        public DateTime DateAdded { get; set; }

        public Rssfeeds Feed { get; set; }
        public ICollection<WordLogs> WordLogs { get; set; }
    }
}
