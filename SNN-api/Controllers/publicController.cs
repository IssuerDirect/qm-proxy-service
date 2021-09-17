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
    [Route("/public")]
    public class publicController : Controller
    {
        apiResponse myResponse = new apiResponse();
        lib clib = new lib();
        SNNLib lib = new SNNLib();
        int pageSize = 24;
        public publicController(IConfiguration config, platformDB platformDB)
        {
            lib.config = config;
            lib.platformDB = platformDB;
            clib.myConfiguration = config;
        }

        [HttpGet("/public/insights")]
        public apiResponse insights(int index = 0)
        {

            return standardMessages.saved;
        }
       
    }
}
