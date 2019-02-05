using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class ParentWords
    {
        public int WordJoinId { get; set; }
        public int ParentWordId { get; set; }
        public int ChildWordId { get; set; }

        public Words ChildWord { get; set; }
        public Words ParentWord { get; set; }
    }
}
