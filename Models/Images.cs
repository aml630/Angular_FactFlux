using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Images
    {
        public int ImageId { get; set; }
        public int ContentId { get; set; }
        public string ImageLocation { get; set; }
        public string ContentType { get; set; }
    }
}
