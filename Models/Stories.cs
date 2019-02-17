using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Stories : Words
    {
        public List<Images> Images { get; set; }
    }

    public partial class PagedResponse
    {
        public List<Stories> Images { get; set; }
        public int PageNumber { get; set; }
    }
}
