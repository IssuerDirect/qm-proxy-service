using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace snn
{
    public class ci_Company
    {
        [Key]
        public int id { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? update_time { get; set; }
        public int ref_Source { get; set; }
        public int ref_Status { get; set; }
        public int? ref_Sector { get; set; }
        public int? ref_CompanyClass { get; set; }
        public string isdr_id { get; set; }
        public string name { get; set; }
        public string ein { get; set; }
        public int? sic_code { get; set; }
        public int? naics_code { get; set; }
        public string ceo { get; set; }
        public string business_description { get; set; }
        public string business_description_long { get; set; }
        public string factset_issuer_id { get; set; }
        public int? employees { get; set; }
        public string profile_data { get; set; }
        public string tos { get; set; }
        public string snapshot { get; set; }
        public string website { get; set; }
        public string phone { get; set; }
        public string founded { get; set; }
        public string email { get; set; }
        public string social { get; set; }
        public string ci_Intacct { get; set; }
        public string ccc { get; set; }
        public string search { get; set; }
        public string iso_country { get; set; }
        public int? ref_Exchange { get; set; }
    }
}
