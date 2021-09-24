using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace snn
{
    public class snn_events
    {
        [Key]
        public int id { get; set; }
        public DateTime? date { get; set; }
        public String title { get; set; }
        public String location { get; set; }

    }
}
