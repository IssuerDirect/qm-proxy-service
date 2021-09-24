using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class snn_videooftheday
    {
        [Key]
        public int id { get; set; }
        public DateTime date { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string link { get; set; }
    }
}
