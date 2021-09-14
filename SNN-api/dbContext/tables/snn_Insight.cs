using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;
using System.ComponentModel;

namespace snn
{
    public class snn_Insight
    {
        [Key]
        public int id { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public int ref_Status { get; set; }
        public int type { get; set; }
        public string src { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        [ForeignKey("type")]
        public virtual ref_InsightType ref_InsightType { get; set; }
    }
}
