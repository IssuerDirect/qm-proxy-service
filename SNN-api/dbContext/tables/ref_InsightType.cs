using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;
using System.ComponentModel;

namespace snn
{
    public class ref_InsightType
    {
        [Key]
        public int id { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public string name { get; set; }
    }
}
