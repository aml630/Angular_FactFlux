using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Rssfeeds
    {
        public Rssfeeds()
        {
            Article = new HashSet<Article>();
        }

        public int FeedId { get; set; }
        public string FeedTitle { get; set; }
        public string FeedLink { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string FeedImage { get; set; }
        public string VideoLink { get; set; }

        public ICollection<Article> Article { get; set; }
    }
}
