using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace snn.Controllers
{

    #region fillingObject
   
    public class Total
    {
        public int value { get; set; }
        public string relation { get; set; }
    }

    public class Query
    {
        public int from { get; set; }
        public int size { get; set; }
    }

    public class Entity
    {
        public string companyName { get; set; }
        public string cik { get; set; }
        public string type { get; set; }
        public string act { get; set; }
        public string fileNo { get; set; }
        public string filmNo { get; set; }
        public string irsNo { get; set; }
        public string stateOfIncorporation { get; set; }
        public string fiscalYearEnd { get; set; }
        public string sic { get; set; }
    }

    public class DocumentFormatFile
    {
        public string sequence { get; set; }
        public string description { get; set; }
        public string documentUrl { get; set; }
        public string type { get; set; }
        public string size { get; set; }
    }

    public class Filing
    {
        public string id { get; set; }
        public string accessionNo { get; set; }
        public string cik { get; set; }
        public string ticker { get; set; }
        public string companyName { get; set; }
        public string companyNameLong { get; set; }
        public string formType { get; set; }
        public string description { get; set; }
        public DateTime filedAt { get; set; }
        public string linkToTxt { get; set; }
        public string linkToHtml { get; set; }
        public string linkToXbrl { get; set; }
        public string linkToFilingDetails { get; set; }
        public List<Entity> entities { get; set; }
        public List<DocumentFormatFile> documentFormatFiles { get; set; }
        public List<object> dataFiles { get; set; }
        public List<object> seriesAndClassesContractsInformation { get; set; }
        public string periodOfReport { get; set; }
        public string effectivenessDate { get; set; }
        public string industry {
            get {
                if (entities != null && entities.Where(e => !string.IsNullOrEmpty(e.sic)).Any()) {
                    var industry = entities.Where(e => !string.IsNullOrEmpty(e.sic)).FirstOrDefault().sic;
                    industry = industry.Replace(industry.Split(" ")[0] + " ", "");
                    return industry;
                }
                return "";
            }
        }
        public string industryID
        {
            get
            {
                if (entities != null && entities.Where(e => !string.IsNullOrEmpty(e.sic)).Any())
                {
                    var myID = entities.Where(e => !string.IsNullOrEmpty(e.sic)).FirstOrDefault().sic;
                    return myID.Split(" ")[0];
                }
                return null;
            }
        }
    }

    public class fillingObject
    {
        public Total total { get; set; }
        public Query query { get; set; }
        public List<Filing> filings { get; set; }
    }
    #endregion

}