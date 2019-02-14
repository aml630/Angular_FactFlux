using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class JobParameter
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public Job Job { get; set; }
    }
}
