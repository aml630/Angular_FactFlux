using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class WordLogs
    {
        public int WordLogId { get; set; }
        public int WordId { get; set; }
        public DateTime DateAdded { get; set; }
        public int ArticleId { get; set; }

        public Article Article { get; set; }
        public Words Word { get; set; }
    }
}
