using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Tweets
    {
        public int TweetId { get; set; }
        public int TwitterUserId { get; set; }
        public string EmbedHtml { get; set; }
        public string TweetText { get; set; }
        public DateTime DateTweeted { get; set; }
        public DateTime DateCreated { get; set; }

        public TwitterUsers TwitterUser { get; set; }
    }
}
