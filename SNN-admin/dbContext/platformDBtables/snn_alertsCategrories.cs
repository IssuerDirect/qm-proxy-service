using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace snn
{
    public class snn_alertsCategrories
    {

        [Key]
        public int id { get; set; }
        public string category  { get; set; }
    }
}
