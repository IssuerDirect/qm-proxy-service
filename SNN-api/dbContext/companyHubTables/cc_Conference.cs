using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class cc_Conference
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public DateTime? create_time { get; set; } = DateTime.Now;
        public DateTime? update_time { get; set; } = DateTime.Now;
        public string title { get; set; }
    }
}
