using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class cc_SnnInsight
    {
        [Key]
        public int id { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
        public DateTime? occur { get; set; }
        public int ref_InsightType { get; set; }
        public int type { get; set; }
        public string src { get; set; }
        public string tags { get; set; }
        public string thumb { get; set; }
        public string keywords { get; set; }
        public string headline { get; set; }
        public string pi_Person { get; set; }
        public string summary { get; set; }
        public string body { get; set; }
        public string author { get; set; }
    }
}
