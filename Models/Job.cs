﻿using System;
using System.Collections.Generic;

namespace FactFluxV3.Models
{
    public partial class Job
    {
        public Job()
        {
            JobParameter = new HashSet<JobParameter>();
            State = new HashSet<State>();
        }

        public int Id { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public string InvocationData { get; set; }
        public string Arguments { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ExpireAt { get; set; }

        public ICollection<JobParameter> JobParameter { get; set; }
        public ICollection<State> State { get; set; }
    }
}
