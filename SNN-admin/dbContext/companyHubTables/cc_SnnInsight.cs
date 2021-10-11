﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace snn
{
    public class cc_SnnInsight
    {
        [Key]
        public string id { get; set; }
        public DateTime? create_time { get; set; } = DateTime.Now;
        public DateTime? update_time { get; set; } = DateTime.Now;
        public DateTime? occur { get; set; }
        public int ref_InsightType { get; set; }
        public string type { get; set; }
        public string src { get; set; }
        public string tags { get; set; }
        public string thumb { get; set; }
        public string keywords { get; set; }
        public string headline { get; set; }
        public string pi_Person { get; set; }
        public string summary { get; set; }
        public string body { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        [ForeignKey("ref_InsightType"), JsonIgnore]
        public virtual ref_InsightType ref_InsightTypeObject { get; set; }
        [NotMapped]
        public string typeTitle {
            get {
                if (ref_InsightTypeObject != null) {
                    return ref_InsightTypeObject.name;
                }
                return null;
            }
        }
    }
}