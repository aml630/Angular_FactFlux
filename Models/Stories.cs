using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Stories : Words
    {
        public List<Images> Images { get; set; }
    }
}
