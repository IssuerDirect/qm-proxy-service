using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace snn
{
    public class cc_SecFiling
    {
        [Key]
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DateTime? create_time { get; set; } = DateTime.Now;
        public DateTime? update_time { get; set; } = DateTime.Now;
        public DateTime? occur { get; set; }
        public string ci_Company { get; set; }
        public int? ref_Sector { get; set; }
        public int? ref_FilingType { get; set; }
        public string accession { get; set; }
        public string form_type { get; set; }
        public string title { get; set; }
        public string src_text { get; set; }
        public string src_xbrl { get; set; }
        public string src_xml { get; set; }

    }
}
