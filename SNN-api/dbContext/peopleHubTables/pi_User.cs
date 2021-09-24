using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class pi_User
    {
        [Key]
        public int id { get; set; }
        public string username { get; set; }
        public string auth_key { get; set; }
        public string password_hash { get; set; }
        public string email { get; set; }
        public string password_reset_token { get; set; }
        public  string settings { get; set; }
        public string ref_Country { get; set; }
        public Int32 level { get; set; }
        public Int32 created_at { get; set; }
        public Int32 updated_at { get; set; }
        public Int32 ref_Status { get; set; }
        public DateTime last_login { get; set; }
    }
}
