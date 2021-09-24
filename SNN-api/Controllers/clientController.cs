using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using net3000;
using Microsoft.Extensions.Configuration;
using net3000.common;
using net3000.common.models;

namespace snn.Controllers
{
    [Route("/client")]
    public class clientController : Controller
    {
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;
        public clientController(IConfiguration config, companyHubDB companyHubDB, peopleHubDB peopleHubDB)
        {
            lib.config = config;
            lib.companyHubDB = companyHubDB;
            lib.peopleHubDB = peopleHubDB;
            clib.myConfiguration = config;
        }

        [HttpPost("/client/alert")]
        public apiResponse insertAlert(snn_alerts alert)
        {
            if (!isClient()) { return standardMessages.unauthorized; } 

            lib.platformDB.snn_alerts.Add(alert);
            lib.platformDB.SaveChanges();
            myResponse = standardMessages.saved;
            myResponse.data = alert.id;
            return standardMessages.saved;
        }

        [HttpPut("/client/alert")]
        public apiResponse updateAlert(snn_alerts alert)
        {
            if (!isClient()) { return standardMessages.unauthorized; } 

            lib.platformDB.snn_alerts.Update(alert);
            lib.platformDB.SaveChanges();
            return standardMessages.saved;
        }

        bool isClient()
        {
            //authenticate user
            return true;
        }
    }
}
