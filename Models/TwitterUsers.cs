using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class TwitterUsers
    {
        public TwitterUsers()
        {
            Tweets = new HashSet<Tweets>();
        }

        public int TwitterUserId { get; set; }
        public string TwitterUsername { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<Tweets> Tweets { get; set; }
    }
}
