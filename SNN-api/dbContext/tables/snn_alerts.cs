using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace snn
{
    public class snn_alerts
    {
        [Key]
        public int id { get; set; }
        public int? categoryID { get; set; }
        public int? userID { get; set; }
        public DateTime? date { get; set; }
        public String details { get; set; }
    }
}
