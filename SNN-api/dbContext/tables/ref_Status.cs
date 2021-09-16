using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace snn
{
    public class ref_Status
    {
        [Key]
        public int id { get; set; }
        public DateTime create_time { get; set; }
        public DateTime update_time { get; set; }
        public string name { get; set; }
    }
}
