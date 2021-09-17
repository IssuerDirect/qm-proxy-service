using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class snn_users
    {
        [Key]
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string country { get; set; }
        public string userid { get; set; }
        public int? logingroupid { get; set; }
        public Guid token { get; set; } = Guid.NewGuid();
    }
}
