using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("categoryID")]
        public snn_alertsCategrories category  { get; set; }
    }
}
