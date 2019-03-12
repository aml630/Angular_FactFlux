using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Words
    {
        public Words()
        {
            ParentWordsChildWord = new HashSet<ParentWords>();
            ParentWordsParentWord = new HashSet<ParentWords>();
            WordLogs = new HashSet<WordLogs>();
        }

        public int WordId { get; set; }
        public string Word { get; set; }
        public DateTime DateCreated { get; set; }
        public bool Banned { get; set; }
        public DateTime? DateIncremented { get; set; }
        public int Daily { get; set; }
        public int Monthly { get; set; }
        public int Yearly { get; set; }
        public bool Main { get; set; }
        public int Type { get; set; }
        public string Description { get; set; }

        public ICollection<ParentWords> ParentWordsChildWord { get; set; }
        public ICollection<ParentWords> ParentWordsParentWord { get; set; }
        public ICollection<WordLogs> WordLogs { get; set; }
    }
}
