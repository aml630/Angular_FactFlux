using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class ParentWithWords : ParentWords
    {
        public List<Words> ChildWords { get; set; }
    }
}
