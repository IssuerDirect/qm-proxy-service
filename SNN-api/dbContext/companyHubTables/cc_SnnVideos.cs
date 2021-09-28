using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace snn
{
    public class cc_SnnVideos
    {
        [Key]
        public int id { get; set; }
        public DateTime? create_time { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string author { get; set; }
    }
}
