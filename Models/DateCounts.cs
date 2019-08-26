using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class DateCounts
    {
        public int DateCountId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OccuranceCount { get; set; }
        public int WordId { get; set; }
    }
}
