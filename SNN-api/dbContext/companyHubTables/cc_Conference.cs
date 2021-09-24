using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class cc_Conference
    {
        [Key]
        public int id { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
        public int ref_Status { get; set; }
        public string ci_Company { get; set; }
        public string title { get; set; }
        public string slug { get; set; }
        public string branding { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public string location { get; set; }
        public string src { get; set; }
        public string industry { get; set; }
        public string region { get; set; }
        public string host { get; set; }
    }
}
