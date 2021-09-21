using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Linq;
using System.ComponentModel;

namespace snn
{
    public class snn_InsightContent
    {
        [Key]
        public int id { get; set; }
        public DateTime date { get; set; } = DateTime.Now;
        public int type { get; set; }
        public string image { get; set; }
        public int ref_Status { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        [ForeignKey("type"), JsonIgnore]
        public virtual ref_InsightType ref_InsightType { get; set; }
        [NotMapped]
        public string typeName {
            get {
                if (ref_InsightType != null) {
                    return ref_InsightType.name;
                }
                return null;
            }
        }
        [ForeignKey("ref_Status"), JsonIgnore]
        public virtual ref_Status ref_StatusObject { get; set; }
        [NotMapped]
        public string statusName
        {
            get
            {
                if (ref_StatusObject != null)
                {
                    return ref_StatusObject.name;
                }
                return null;
            }
        }
    }
}
