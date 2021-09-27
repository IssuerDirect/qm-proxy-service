using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace snn
{
    public class cc_SnnVideos
    {
        [Key]
        public string id { get; set; }
        public DateTime create_time { get; set; } = DateTime.Now;
        public string title { get; set; }
        public string link { get; set; }
        public string author { get; set; }
    }
}
